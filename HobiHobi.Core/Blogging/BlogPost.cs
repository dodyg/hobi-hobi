using HobiHobi.Core.Framework;
using HobiHobi.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Blogging
{
    public class BlogPost
    {
        public static Key NewId(string value = null)
        {
            return Key.Generate("Blog/Feed/Post/", value);
        }

        public string Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime LastModified { get; set; }
        public string FeedId { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public bool AllowComment { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<string> Tags { get; set; }

        public BlogPost()
        {
            DateCreated = Stamp.Time();
            DatePublished = Stamp.Time();
            LastModified = Stamp.Time();
            Tags = new List<string>();
            AllowComment = true;
            IsDeleted = false;
        }

        public void GenerateSlug()
        {
            if (Title.IsNullOrWhiteSpace())
                GenerateNumberSlug();
            else
                GenerateTitleSlug();
        }

        public void GenerateTitleSlug()
        {
            Slug = Texts.ConvertTitleToUrl(Title);
        }

        public void GenerateNumberSlug()
        {
            Slug = DateTime.UtcNow.Ticks.ToString();
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags = Tags.Where(x => x != tag).ToList();
        }
    }
}
