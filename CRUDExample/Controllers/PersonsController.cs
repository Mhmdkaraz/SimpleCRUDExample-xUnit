﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers {
    [Route("[controller]")]
    public class PersonsController : Controller {
        //private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService, ICountriesService countriesService) {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        [Route("[action]")]
        [Route("/")]
        public IActionResult Index(string searchBy, string searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC) {
            //searching
            ViewBag.SearchFields = new Dictionary<string, string>() {
                {nameof(PersonResponse.PersonName),"Person Name" },
                {nameof(PersonResponse.Email),"Email" },
                {nameof(PersonResponse.DateOfBirth),"Date of Birth" },
                {nameof(PersonResponse.Gender),"Gender" },
                {nameof(PersonResponse.CountryId),"Country" },
                {nameof(PersonResponse.Address),"Address" }
            };
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }

        //Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create() {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp => new SelectListItem() {
                Text=temp.CountryName,Value=temp.CountryId.ToString()
            });
            
            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest) {
            if (!ModelState.IsValid) {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index","Persons");
        }

        [Route("[action]/{personId}")]
        [HttpGet]
        public IActionResult Edit(Guid personId) {
            PersonResponse personResponse = _personsService.GetPersonByPersonId(personId);
            if(personResponse==null) {
                return RedirectToAction("Index", "Persons");
            }
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp=>new SelectListItem() {
                Text=temp.CountryName, Value=temp.CountryId.ToString()
            });

            return View(personUpdateRequest);
        }

        [Route("[action]/{personId}")]
        [HttpPost]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest) {
            PersonResponse personResponse = _personsService.GetPersonByPersonId(personUpdateRequest.PersonId);
            if(personResponse==null) {
                return RedirectToAction("Index", "Persons");
            }
            if(ModelState.IsValid) {
                PersonResponse updatedPerson = _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index", "Persons");
            } else {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp => new SelectListItem() {
                    Text = temp.CountryName, Value = temp.CountryId.ToString()
                });
                ViewBag.Errors = ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage);
                return View(personResponse.ToPersonUpdateRequest());
            }
        }

        [Route("[action]/{personId}")]
        [HttpGet]
        public IActionResult Delete(Guid? personId) {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personId);
            if(personResponse == null) {
                return RedirectToAction("Index", "Persons");
            }
            return View(personResponse);
        }
        [Route("[action]/{personId}")]
        [HttpPost]
        public IActionResult Delete(PersonUpdateRequest? personUpdateRequest) {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personUpdateRequest?.PersonId);
            if (personResponse == null) 
                return RedirectToAction("Index", "Persons");
            
            _personsService.DeletePerson(personUpdateRequest?.PersonId);
            return RedirectToAction("Index", "Persons");
        }
    }
}
