using System.Net.Http.Json;

namespace BaseMigrationUI.Helper
{
    public class ResponseMessageUtile
    {
        public async Task<T?> HandleResponse<T>(HttpResponseMessage? response)
        {

            if (response == null)
            {
                throw new SystemException("Response message is null.");
            }

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                throw new SystemException(errorMessage);
            }

            var result = await response!.Content.ReadFromJsonAsync<T>();

            if (result == null)
            {
                throw new SystemException("Response model is null.");
            }

            return result;
        }
    }
}
