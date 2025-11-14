using System.Net.Http.Headers;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class AuthenticatedHttpMessageHandler : DelegatingHandler
{
    private readonly IAuthService _auth;
    private static readonly SemaphoreSlim _refreshLock = new(1, 1);

    public AuthenticatedHttpMessageHandler(IAuthService auth) => _auth = auth;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Adjunta token si existe y no ha expirado
        var token = await _auth.GetAccessTokenAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // Intenta refrescar de forma coordinada
            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                // Puede que otro hilo ya refrescara
                var freshToken = await _auth.GetAccessTokenAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(freshToken))
                {
                    var ok = await _auth.RefreshAsync(cancellationToken);
                    if (!ok) return response; // falló refresco, devuelve 401
                    freshToken = await _auth.GetAccessTokenAsync(cancellationToken);
                }

                if (!string.IsNullOrWhiteSpace(freshToken))
                {
                    response.Dispose(); // desecha respuesta 401 previa
                    var cloned = await CloneHttpRequestMessageAsync(request);
                    cloned.Headers.Authorization = new AuthenticationHeaderValue("Bearer", freshToken);
                    return await base.SendAsync(cloned, cancellationToken);
                }
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        return response;
    }

    // Clonado básico de la request para reintentar
    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        // Copiar contenido si existe
        if (request.Content is not null)
        {
            var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);

            // Copiar cabeceras de contenido
            foreach (var h in request.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }

        // Copiar cabeceras
        foreach (var prop in request.Headers)
            clone.Headers.TryAddWithoutValidation(prop.Key, prop.Value);

        clone.Version = request.Version;
        clone.VersionPolicy = request.VersionPolicy;

        return clone;
    }
}