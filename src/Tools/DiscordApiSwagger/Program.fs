open Microsoft.AspNetCore.Builder
open NSwag.AspNetCore
open System
open System.Net.Http
open System.Threading.Tasks

let app = WebApplication.CreateBuilder().Build()

app.MapGet("/swagger/v10/swagger.json", Func<Task<string>>(fun () -> task {
    use httpClient = new HttpClient()
    use req = new HttpRequestMessage(
        HttpMethod.Get,
        "https://raw.githubusercontent.com/discord/discord-api-spec/refs/heads/main/specs/openapi.json"
    )

    let! res = httpClient.SendAsync(req)
    return! res.Content.ReadAsStringAsync()
})) |> ignore

let route = SwaggerUiRoute("v10", "/swagger/v10/swagger.json")
app.UseSwaggerUi(_.SwaggerRoutes.Add(route)) |> ignore

app.Run()
