using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyTypesServices
{
    public class GetAllTest : BaseTest
    {
        [Test]
        public async Task CurrencyTypes_GetAll_IsOk()
        {
            var _query = new Application.Function.CurrencyTypes.Queries.GetAll.GetAllCurrencyTypesQuery();
            var _handler = new Application.Function.CurrencyTypes.Queries.GetAll.GetAllCurrencyTypesHandler(_currencyTypesRepository.Object, _mapper);

            var result = await _handler.Handle(_query, default);

            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(3);
        }
    }
}
