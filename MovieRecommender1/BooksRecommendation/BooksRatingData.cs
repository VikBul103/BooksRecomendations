using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace BooksRecommendation
{
    public class BooksRating
    {
        [LoadColumn(0)]
        public float user_id;
        [LoadColumn(1)]
        public float book_id;
        [LoadColumn(2)]
        public float Label;
    }
    public class BooksRatingPrediction
    {
        public float Label;
        public float Score;
    }
    //public class BooksRating
    //{
    //    public int Id;
    //    public string Name;
    //    RatingDist1,pagesNumber,RatingDist4,RatingDistTotal,PublishMonth,PublishDay,Publisher,CountsOfReview,PublishYear,Language,Authors,Rating,RatingDist2,RatingDist5,ISBN,RatingDist3
    //}
}
