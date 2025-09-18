using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("v1/user")]
public class User : ControllerBase
{

    [HttpGet("{id:int}")]
    public IActionResult getUser(int id)
    {

        return Ok(new { id = id, name = "Ahmed", email = "ahmed.mamdouh.elgndy@gmail.com" });

    }

    [HttpGet("query")]
    public IActionResult getbyQuery([FromQuery] userInfo userInfo)
    {

        return Ok(new { name = userInfo.name, age = userInfo.age });

    }

    [HttpGet("header")]
    public IActionResult getByHeader([FromHeader(Name = "X-language")] string lan)
    {
        return Ok($"you are using {lan} Language");

    }

    [HttpPost("from")]
    public IActionResult getByForm([FromForm] string password, [FromForm] string userName, [FromForm] int age)
    {
        return Ok(new { satus = 200, userName = userName, age = age });

    }


    [HttpPost("body")]
    public IActionResult getByBody([FromBody] userInfoBody userInfo)
    {
        return Ok(userInfo);

    }


    [HttpPost("cookie")]
    public IActionResult getByBody()
    {
        // Read cookies from request
        var theme = HttpContext.Request.Cookies["theme"];
        var language = HttpContext.Request.Cookies["language"];
        var timezone = HttpContext.Request.Cookies["timezone"];
        var token = HttpContext.Request.Cookies["access_token"];
        
        return Ok(new
    {
        Theme = theme,
        Language = language,
        Timezone = timezone,
        AccessToken = token
    });
    }
}

public class userInfo
{
    public string name { get; set; }
    public int age {  get; set; }
}

public class userInfoBody
{
    public string name { get; set; }
    public int age { get; set; }
    
    public string email { get; set; }
}

