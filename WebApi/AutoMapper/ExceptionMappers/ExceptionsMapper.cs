﻿using Core.Entities;
using Core.ViewModels.ExceptionViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ExceptionMapper;

public class ExceptionsMapper : IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>
{
    public IEnumerable<ExceptionEntityReadViewModel> Map(IEnumerable<ExceptionEntity> source)
    {
        var exceptionEntityReadViewModels = source.Select(GetExceptionEntityReadViewModel).ToList();
        return exceptionEntityReadViewModels;
    }

    private ExceptionEntityReadViewModel GetExceptionEntityReadViewModel(ExceptionEntity exception)
    {
        var exceptionViewModel = new ExceptionEntityReadViewModel()
        {
            Id = exception.Id,
            DateTime = exception.DateTime,
            Name = exception.Name,
            Path = exception.Path
        };

        return exceptionViewModel;
    }
}


