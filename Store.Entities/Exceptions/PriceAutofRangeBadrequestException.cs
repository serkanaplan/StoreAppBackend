namespace Store.Entities.Exceptions;

public class PriceAutofRangeBadrequestException() : BadRequestException("Maksimum Fiyat 1000'den küçük 10'dan büyük olmalı")
{
}