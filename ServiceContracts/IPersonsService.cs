﻿using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts {
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>
        /// Returns the same person details, along with the newly generated PersonId
        /// </returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>
        /// Returns a list of objects of PersonResponse type
        /// </returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person Id
        /// </summary>
        /// <param name="personId">Person id to search</param>
        /// <returns>
        /// Returns matching person object
        /// </returns>
        PersonResponse GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>
        /// Returns all matching persons based on the given search field and search string
        /// </returns>
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of property (key), based on which the persons shoulf be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified person details based on the given perosnId
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update including person id</param>
        /// <returns>
        /// Returns the person response object after updation
        /// </returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personId">PersonId to delete</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        bool DeletePerson(Guid? personId);
    }
}
