using Microsoft.AspNetCore.Mvc;

[Route("/error")]
public class ErrorsController : ControllerBase
{
    public ErrorsController()
    {
        Console.WriteLine("Errors Controller");

    }

    public IActionResult Error()
    {
        return Problem();
    }
}