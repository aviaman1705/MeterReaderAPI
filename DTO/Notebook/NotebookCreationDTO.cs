﻿using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO.Notebook
{
    public class NotebookCreationDTO
    {

        [Required(ErrorMessage = "the field with name {0} is required")]
        [RegularExpression("([0-9999]+)", ErrorMessage = "Please enter valid Number")]
        public int Number { get; set; }
    }
}
