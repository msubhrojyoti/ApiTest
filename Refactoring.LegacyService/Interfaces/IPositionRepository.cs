using System;
using System.Collections.Generic;
using System.Text;

namespace Refactoring.LegacyService.Interfaces
{
    public interface IPositionRepository
    {
        /// <summary>
        /// Gets the position using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Position GetById(int id);
    }
}
