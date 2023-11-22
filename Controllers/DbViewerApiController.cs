using Microsoft.AspNetCore.Mvc;
using SkbKontur.DbViewer;
using SkbKontur.DbViewer.Configuration;
using SkbKontur.DbViewer.DataTypes;

namespace DbViewerExample.Controllers;

[ApiController]
[Route("db-viewer")]
public class DbViewerApiController : ControllerBase
{
    private readonly DbViewerApi _impl;

    public DbViewerApiController(SchemaRegistryProvider schemaRegistryProvider)
    {
        _impl = new DbViewerApi(schemaRegistryProvider.GetSchemaRegistry());
    }

    [HttpGet]
    [Route("names")]
    public ObjectIdentifier[] GetNames()
    {
        return _impl.GetNames();
    }

    [HttpGet]
    [Route("{objectIdentifier}/meta")]
    public ObjectDescription GetMeta(string objectIdentifier)
    {
        return _impl.GetMeta(objectIdentifier);
    }

    [HttpPost]
    [Route("{objectIdentifier}/search")]
    public Task<SearchResult> SearchObjects(string objectIdentifier, [FromBody] ObjectSearchRequest query)
    {
        return _impl.SearchObjects(objectIdentifier, query, IsSuperUser());
    }

    [HttpPost]
    [Route("{objectIdentifier}/count")]
    public Task<CountResult> CountObjects(string objectIdentifier, [FromBody] ObjectSearchRequest query)
    {
        return _impl.CountObjects(objectIdentifier, query, IsSuperUser());
    }

    [HttpGet]
    [Route("{objectIdentifier}/download/{queryString}")]
    public async Task<IActionResult> DownloadObjects(string objectIdentifier, string queryString)
    {
        var fileInfo = await _impl.DownloadObjects(objectIdentifier, queryString, IsSuperUser())
            .ConfigureAwait(false);
        return File(fileInfo.Content, fileInfo.ContentType, fileInfo.Name);
    }

    [HttpPost]
    [Route("{objectIdentifier}/details")]
    public Task<ObjectDetails> ReadObject(string objectIdentifier, [FromBody] ObjectReadRequest query)
    {
        return _impl.ReadObject(objectIdentifier, query);
    }

    [HttpDelete]
    [Route("{objectIdentifier}/delete")]
    public Task DeleteObject(string objectIdentifier, [FromBody] ObjectReadRequest query)
    {
        return _impl.DeleteObject(objectIdentifier, query, IsSuperUser());
    }

    [HttpPost]
    [Route("{objectIdentifier}/update")]
    public Task UpdateObject(string objectIdentifier, [FromBody] ObjectUpdateRequest query)
    {
        return _impl.UpdateObject(objectIdentifier, query, IsSuperUser());
    }

    private bool IsSuperUser()
    {
        return HttpContext.Request.Cookies.ContainsKey("isSuperUser");
    }
}