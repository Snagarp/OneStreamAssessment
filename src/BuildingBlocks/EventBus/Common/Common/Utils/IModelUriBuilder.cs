//2023 (c) TD Synnex - All Rights Reserved.

namespace Common.Utils
{
    public interface IModelUriBuilder
    {
        Uri Build(HttpClient client, string path, object model, bool suppressQueryParameters = false);
    }
}
