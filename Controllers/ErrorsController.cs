using Microsoft.AspNetCore.Mvc;

[Route("/error")]
public class ErrorsController : ControllerBase
{

    public IActionResult Error()
    {
        return Problem();
    }
}