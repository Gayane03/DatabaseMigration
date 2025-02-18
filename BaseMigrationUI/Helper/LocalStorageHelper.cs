using Microsoft.JSInterop;

namespace BaseMigrationUI.Helper
{
	public class LocalStorageHelper
	{
		private readonly IJSRuntime jsRuntime;

		public LocalStorageHelper(IJSRuntime jsRuntime)
		{
			this.jsRuntime = jsRuntime;
		}
		public async Task SaveToken(string tokenType, string token)
		{
			
		    await RemoveToken(tokenType);
		
			await jsRuntime.InvokeVoidAsync("localStorage.setItem", tokenType, token);
		}
		public async Task<string?> GetToken(string tokenType)
		{
			return await jsRuntime.InvokeAsync<string>("localStorage.getItem", tokenType);
		}

		public async Task RemoveToken(string tokenType)
		{
			var token = await GetToken(tokenType);

			if(!string.IsNullOrEmpty(token))
			{
				await jsRuntime.InvokeVoidAsync("localStorage.removeItem", tokenType);
			}
		}

	}
}
