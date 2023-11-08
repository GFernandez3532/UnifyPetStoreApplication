# My Unify Pet Store Application

A simple application to fetch and display available pets from a pet store.


## Installation

1. Open Project in Visual Studio.
2. Start the Application.


## Usage

To fetch and display available pets, simply run the Application. Any exception or issue should be handled accordingly.

## System Requirements

- .NET Framework version 6.0
- Supported platforms: Windows, macOS, Linux

## License

This project is licensed under the [Unify License](https://unifysolutions.net/)



## ChatGPT Snippets

Here are some code snippets and insights from ChatGPT.


### Sorting and Grouping Pets

Using LINQ is a great approach for sorting a list of pets by categories and then displaying them in reverse order by name. Here's an example of how you can achieve this:

```c#
if (pets != null)
{
    var sortedPets = pets
        .GroupBy(pet => pet.Category.Name)
        .OrderBy(group => group.Key)
        .SelectMany(group => group.OrderByDescending(pet => pet.Name))
        .ToList();
}
```

### Catching exception improvements.

You're correct that you are catching the `HttpRequestException` for both the initial HTTP request and the JSON deserialization step. You can improve this by catching the `HttpRequestException` once and then handling the JSON deserialization exception separately.

By catching `HttpRequestException` once, you're effectively handling any issues related to the API request. If the request is successful but there's an issue with JSON deserialization, that will be caught by the inner `catch (JsonException ex)` block. This way, you have a cleaner exception handling structure without repeating the same exception handling logic.

## Regular expression check

The regular expression @"\{[a-zA-Z0-9]*\}" is used to find and match any substring in a text that starts with an opening curly brace {, followed by zero or more alphanumeric characters (letters or digits), and ends with a closing curly brace }.

For example, it would match strings like {abc}, {123}, {AaBb1}, and so on. It's commonly used for extracting text within curly braces that follows a specific pattern in a larger text document.

## Final touches and review

Your code looks good, and it effectively separates the concerns of making the API request, deserializing JSON, and handling exceptions. This approach should work well for your application. 

You're now handling the creation of the `petsFromAPI` list properly before deserializing JSON, which helps ensure that you won't get a null reference exception. Additionally, by catching `JsonException` and `HttpRequestException` separately, you have a clear and structured approach to handle different types of exceptions that can occur during the process.

Your method now returns the `groupedPets` dictionary, which is populated with grouped and ordered data as per your requirements. It should work as expected.



### Further steps I would take to have it production ready:

* Implement SQL Injection protection when working with the DB.
* Further data Sanitation from the API. (check for more invalid characters, blacklisted words, numbers in names, etc)
* More thorough testing. Multiple scenarios when testing the grouping by category. Better testing of the connection and request.
* The ability to choose which the status of pets when retrieving information.
* Better exception and error handling. Logging of errors in database or file.


