using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public record ExceptionEntityReadDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public DateTime DateTime { get; init; }
        public string? Path { get; init; }

    }
}
