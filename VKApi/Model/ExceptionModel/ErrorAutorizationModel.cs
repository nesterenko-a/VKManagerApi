namespace VKApi.Model
{
    //В случаях, когда требуется дополнительная проверка пользователя, например при авторизации из подозрительного места, будет возвращена следующая ошибка:
    //Необходимо открыть браузер со страницей, указанной в поле redirect_uri и ждать, пока пользователь будет направлен на страницу blank.html 
    //с параметром success=1 в случае успеха и fail=1 в случае, если пользователь отменил проверку своего аккаунта. 
    public class Error2FAutorizationModel//: AbstractRequesModel
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string redirect_uri { get; set; }
    }

    public class ErrorCapchaAutorizationModel//: AbstractRequesModel
    {
        public string error { get; set; }
        public string captcha_sid { get; set; }
        public string captcha_img { get; set; }
    }
}
