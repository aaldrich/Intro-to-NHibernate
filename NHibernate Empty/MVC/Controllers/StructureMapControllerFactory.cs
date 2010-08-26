using System;
using System.Web.Mvc;
using StructureMap;

namespace NHibernateDemo.MVC.Controllers
{
    /// <summary>
    /// Override the default MVC Controller Factory.
    /// </summary>
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(Type controllerType)
        {
            try
            {
                if (controllerType == null)
                    return null;

                return ObjectFactory.GetInstance(controllerType) as Controller;
            }
            catch (StructureMapException)
            {
                System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                throw;
            }
        }
    }
}