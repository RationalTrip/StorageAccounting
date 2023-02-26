using FluentAssertions;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace StorageAccounting.WebAPI.Tests.TestExtensions
{
    internal static class ObjectAssertionExtensions
    {
        public static void BeObjectResult(this ObjectAssertions objResult,
            int expectedStatusCode,
            object equivalentValue)
        {
            var actualObjResult = objResult.Subject as ObjectResult;

            actualObjResult.Should().NotBeNull();

            actualObjResult!.StatusCode.Should().Be(expectedStatusCode);
            actualObjResult!.Value.Should().BeEquivalentTo(equivalentValue);

        }

        public static void BeStatusCodeResult(this ObjectAssertions statusCodeResult,
            int expectedStatusCode)
        {
            var actualObjResult = statusCodeResult.Subject as StatusCodeResult;

            actualObjResult.Should().NotBeNull();

            actualObjResult!.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}
