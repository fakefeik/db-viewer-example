using System.Reflection;
using GroboContainer.Core;
using GroboContainer.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DbViewerExample;

public class GroboControllerFactory : IControllerFactory
{
    private readonly IContainer _groboContainer;

    public GroboControllerFactory()
    {
        var entryAssembly = Assembly.GetEntryAssembly().Location;
        var assemblies = Directory
            .EnumerateFiles(Path.GetDirectoryName(entryAssembly), "*.dll", SearchOption.TopDirectoryOnly)
            .Where(x => Path.GetFileName(x).StartsWith("DbViewerExample"))
            .Select(Assembly.LoadFrom);
        _groboContainer = new Container(new ContainerConfiguration(assemblies));
    }

    public object CreateController(ControllerContext controllerContext)
    {
        var controllerType = controllerContext.ActionDescriptor.ControllerTypeInfo.AsType();
        var controller = _groboContainer.Create(controllerType);
        ((ControllerBase)controller).ControllerContext = controllerContext;
        return controller;
    }

    public void ReleaseController(ControllerContext context, object controller)
    {
    }
}