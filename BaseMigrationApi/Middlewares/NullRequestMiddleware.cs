namespace BaseMigrationApi.Middlewares
{
	public class NullRequestMiddleware
	{

		private readonly RequestDelegate _next;

		public NullRequestMiddleware(RequestDelegate next)
		{
			_next = next;
		}



		public async Task Invoke(HttpContext context)
		{
			// Check if the request has a body
			if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Response.ContentType = "application/json";

				var errorResponse = new
				{
					message = "Request body cannot be null or empty."
				};

				await context.Response.WriteAsJsonAsync(errorResponse);
				return;
			}

			await _next(context); // Call the next middleware in the pipeline
		}
	}
}
