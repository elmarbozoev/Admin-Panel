﻿using AdminPanel.Interfaces;

namespace AdminPanel.Models
{
    public class News : IContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Picture>? Pictures { get; set; }
        public int MainPictureIndex { get; set; }
        public DateTime DateOfPublication { get; set; }

        public News()
        {
            Pictures = new List<Picture>();
        }

    }
}
