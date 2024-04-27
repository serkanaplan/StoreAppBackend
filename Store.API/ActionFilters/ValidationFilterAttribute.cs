using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Store.API;

//action metod çalışmadan önce,sonra veya çalıştığı anda gerçekleşmesini istediğimiz işlemler için actionfilter yazarız 
//burada yaptığımız işlem cretate,update ve delete işlemlerinde hepsinde kontrol ettiğimiz parametrenin boş olup olmama durumunu daha action a uğramadan burda yakalamak ve gerekli işlemi gerçekleştirmek
//yoksa öbür türlü 3 metod için de aynı işlemleri if ile metod içerisinde kontrol edyorduk. işte book nesnesi boş mu falan
public class ValidationFilterAttribute :ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
         var controller = context.RouteData.Values["controller"];//kontoller sınıfına eriştim
            var action = context.RouteData.Values["action"];//action metodlara eriştim

            // Action metoda DTO içeren parametrelere eriştim
            var param = context.ActionArguments.SingleOrDefault(p => p.Value.ToString().Contains("DTO")).Value;

            if(param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. " +
                    $"Controller : {controller} " +
                    $"Action :  {action}" );
                return; // 400
            }

            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        
    }

}
