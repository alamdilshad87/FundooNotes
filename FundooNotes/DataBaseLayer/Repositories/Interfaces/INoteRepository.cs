using DataBaseLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLayer.Repositories.Interfaces
{
    internal interface INoteRepository
    {
        Task<Note> CreateAsync(Note note);
        Task<IEnumerable<Note>> GetAllAsync();
    }

}
