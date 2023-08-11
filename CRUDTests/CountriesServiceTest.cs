using System;
using System.Collections.Generic;
using System.Reflection;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests {
    public class CountriesServiceTest {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest() {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry
        //when CountryAddRequset is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry() {
            //arrange
            CountryAddRequest? request = null;
            //assert
            Assert.Throws<ArgumentNullException>(() => {
                //act
                _countriesService.AddCountry(request);
            });

        }
        //When the CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull() {
            //arrange
            CountryAddRequest? request = new CountryAddRequest() {
                CountryName = null
            };
            //assert
            Assert.Throws<ArgumentException>(() => {
                //act
                _countriesService.AddCountry(request);
            });

        }
        //When the CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName() {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest() {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest() {
                CountryName = "USA"
            };
            //assert
            Assert.Throws<ArgumentException>(() => {
                //act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);

            });

        }
        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails() {
            //arrange
            CountryAddRequest? request = new CountryAddRequest() {
                CountryName = "Japan"
            };

            //act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();
            //assert
            Assert.True(response.CountryId != Guid.Empty);
            //calls Equals method internally
            Assert.Contains(response, countries_from_GetAllCountries);

        }
        //objA.Equals(objB) (compares referece in case of the object)
        #endregion

        #region GetAllCountries
        //The list should be empty by default (before adding any countries)
        [Fact]
        public void GetAllCountries_EmptyList() {
            //Act
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_country_response_list);
        }
        [Fact]
        public void GetAllCountries_AddFewCountries() {
            List<CountryAddRequest> country_request_list =
                new List<CountryAddRequest> {
                    new CountryAddRequest() { CountryName = "USA" },
                    new CountryAddRequest() { CountryName = "UK" }
                };
            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach (CountryAddRequest country_add_request in country_request_list) {
                countries_list_from_add_country.Add
                    (_countriesService.AddCountry(country_add_request));
            }
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();
            //Assert
            foreach (CountryResponse expected_country in countries_list_from_add_country) {
                Assert.Contains(expected_country, actual_country_response_list);
            }
        }
        #endregion

        #region GetCountryByCountryId
        //If we supply null as CountryId, it should return null as CountryResponse
        [Fact]
        public void GetCountryByCountryId_NullCountryId() {
            //Arrange
            Guid? countryId = null;
            //Act
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);
            //Assert
            Assert.Null(country_response_from_get_method);
        }
        //If we supply a valid country id, it should return the matching country details as CountryResponse object 
        [Fact]
        public void GetCountryByCountryId_ValidCountryId() {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() {
                CountryName = "China"
            };
            CountryResponse country_response_from_add_request = _countriesService.AddCountry(country_add_request);
            //Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryId(country_response_from_add_request.CountryId);
            //Assert
            Assert.Equal(country_response_from_add_request, country_response_from_get);

        }
        #endregion

    }
}
