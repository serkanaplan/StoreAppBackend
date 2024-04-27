using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.Entities.DTOS;
using Store.Entities.Models;
using Store.Repo.Contracts;
using Store.Service.Contracts;

namespace Store.Service.Concretes;

public class ServiceManager(IBookService bookService, ICategoryService categoryService, IAuthenticationService authenticationService) : IServiceManager
{
    private readonly IBookService _BookService = bookService;
    private readonly ICategoryService _CategoryService = categoryService;
    private readonly IAuthenticationService _AuthenticationService = authenticationService;

    public IBookService BookService => _BookService;
    public ICategoryService CategoryService => _CategoryService;
    public IAuthenticationService AuthenticationService => _AuthenticationService;

}