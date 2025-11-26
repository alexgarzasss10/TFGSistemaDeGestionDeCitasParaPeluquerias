from fastapi import APIRouter, HTTPException, Query, Depends, Request
from datetime import datetime, timezone
from sqlmodel import Session, select

from app.db import get_session
from app.helpers.db_memory import DB
from app.models.review import Review, CreateReview
from app.models.review import ReviewTable as ReviewDB
from app.helpers.urls import ensure_absolute

router = APIRouter(prefix="/reviews", tags=["reviews"])


def _iso_z(dt: datetime) -> str:
    if dt.tzinfo is None:
        dt = dt.replace(tzinfo=timezone.utc)
    return dt.astimezone(timezone.utc).replace(microsecond=0).isoformat().replace("+00:00", "Z")


def _to_legacy(r: ReviewDB) -> Review:
    return Review(
        id=r.id,
        barberId=r.barberId,
        serviceId=r.serviceId,
        rating=r.rating,
        comment=r.comment,
        userName=r.userName,
        createdAt=_iso_z(r.createdAt),
        userPhotoUrl=getattr(r, "userPhotoUrl", None),
    )

def _abs(request: Request, review: Review) -> Review:
    # Normaliza userPhotoUrl a absoluta si es relativa
    review.userPhotoUrl = ensure_absolute(review.userPhotoUrl, str(request.base_url)) if review.userPhotoUrl else review.userPhotoUrl
    return review


@router.get("", summary="Listado de reviews (filtradas opcionalmente)", response_model=list[Review])
def get_reviews(
    request: Request,
    barberId: int | None = Query(None),
    serviceId: int | None = Query(None),
    session: Session = Depends(get_session),
):
    items_db = session.exec(select(ReviewDB)).all()
    if items_db:
        reviews = items_db
        if barberId is not None:
            reviews = [r for r in reviews if r.barberId == barberId]
        if serviceId is not None:
            reviews = [r for r in reviews if r.serviceId == serviceId]
        reviews = sorted(reviews, key=lambda r: r.createdAt, reverse=True)
        return [_abs(request, _to_legacy(r)) for r in reviews]

    # Fallback memoria
    reviews_mem = DB["reviews"]
    if barberId is not None:
        reviews_mem = [r for r in reviews_mem if r["barberId"] == barberId]
    if serviceId is not None:
        reviews_mem = [r for r in reviews_mem if r["serviceId"] == serviceId]
    reviews_mem = sorted(reviews_mem, key=lambda r: r.get("createdAt", ""), reverse=True)
    # Normalizar en fallback memoria
    result: list[Review] = []
    base = str(request.base_url)
    for rm in reviews_mem:
        photo = rm.get("userPhotoUrl") or rm.get("photoUrl")  # distintos seeds posibles
        if photo and not (photo.startswith("http://") or photo.startswith("https://")):
            rm = {**rm, "userPhotoUrl": ensure_absolute(photo, base)}
        result.append(rm)  # tipo dict compatible con response_model
    return result


@router.get("/{review_id}", summary="Detalle de review", response_model=Review)
def get_review(review_id: int, request: Request, session: Session = Depends(get_session)):
    r = session.get(ReviewDB, review_id)
    if r:
        return _abs(request, _to_legacy(r))
    # Fallback memoria
    m = next((x for x in DB["reviews"] if x["id"] == review_id), None)
    if not m:
        raise HTTPException(status_code=404, detail="No existe la review")
    photo = m.get("userPhotoUrl") or m.get("photoUrl")
    if photo and not (photo.startswith("http://") or photo.startswith("https://")):
        m = {**m, "userPhotoUrl": ensure_absolute(photo, str(request.base_url))}
    return m


@router.post("", status_code=201, summary="Crear review (solo SQL)", response_model=Review)
def create_review(payload: CreateReview, request: Request, session: Session = Depends(get_session)):
    r = ReviewDB(
        barberId=payload.barberId,
        serviceId=payload.serviceId,
        rating=payload.rating,
        comment=payload.comment,
        userName=payload.userName,
        userPhotoUrl=getattr(payload, "userPhotoUrl", None),  # NUEVO
        createdAt=datetime.now(timezone.utc),
    )
    session.add(r)
    session.commit()
    session.refresh(r)
    return _abs(request, _to_legacy(r))


@router.put("/{review_id}", summary="Actualizar review (solo SQL)", response_model=Review)
def update_review(review_id: int, payload: ReviewDB, request: Request, session: Session = Depends(get_session)):
    r = session.get(ReviewDB, review_id)
    if not r:
        raise HTTPException(status_code=404, detail="No existe la review (SQL)")
    for field in ["barberId", "serviceId", "rating", "comment", "userName", "userPhotoUrl"]:  # Añadido userPhotoUrl
        val = getattr(payload, field, None)
        if val is not None:
            setattr(r, field, val)
    session.add(r)
    session.commit()
    session.refresh(r)
    return _abs(request, _to_legacy(r))


@router.delete("/{review_id}", summary="Eliminar review (solo SQL)", status_code=204)
def delete_review(review_id: int, session: Session = Depends(get_session)):
    r = session.get(ReviewDB, review_id)
    if not r:
        raise HTTPException(status_code=404, detail="No existe la review (SQL)")
    session.delete(r)
    session.commit()
    return None