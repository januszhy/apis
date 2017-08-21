using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebAPI_1;
using WebAPI_1.Controllers;

namespace WebAPI_1.Tests.Controllers
{
    [TestClass]
    public class hwControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            hwController controller = new hwController();
            //controller.Post("Hello World");
            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Hello World", result.ElementAt(0).msg);
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            hwController controller = new hwController();

            // Act
            var result = controller.Get(0);
            var compareString = result.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual("Hello World", compareString);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            hwController controller = new hwController();

            // Act
            var result = controller.Post("value");
            var compareString = result.Headers.Location.ToString();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("http://api/hw/2", compareString);
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            hwController controller = new hwController();

            // Act
            var result = controller.Put(1, "After value");
            var compareString = result.Content.ReadAsStringAsync().Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("After value", compareString);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            hwController controller = new hwController();

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
