using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.Test.Fixtures;

public class ImageServiceFixture
{
    public ImageServiceFixture()
    {
        var fixture =
            new Fixture().Customize(new AutoMoqCustomization());

        MockConfiguration = fixture.Freeze<Mock<IConfiguration>>();
        MockImageRepository = fixture.Freeze<Mock<IImageRepository>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockMemoryCache = fixture.Freeze<Mock<IMemoryCache>>();
        MockFormFile = new Mock<IFormFile>();
        
    BodyWithNoImages = GetBodyWithNoImages();
    BodyWithThreeInnerImages = GetBodyWithThreeInnerImages();
    BodyWithThreeInnerAndOuterImages = GetBodyWithThreeInnerAndOuterImages();
    TrimmedBodyWithThreeInnerAndOuterImages = GetTrimmedBodyWithThreeInnerAndOuterImages();
    BodyWithThreeInnerAndOuterWithQueryImages = GetBodyWithThreeInnerAndOuterWithQueryImages();
    TrimmedBodyWithThreeInnerAndOuterWithQueryImages = GetTrimmedBodyWithThreeInnerAndOuterWithQueryImages();    
    BodyWithThreeInnerAndOuterWithQueryAndAttributesImages = GetBodyWithThreeInnerAndOuterWithQueryAndAttributesImages();
    TrimmedBodyWithThreeInnerAndOuterWithQueryAndAttributesImages = GetTrimmedBodyWithThreeInnerAndOuterWithQueryAndAttributesImages();
    ChangedBodyWithTheSameImages = GetChangedBodyWithTheSameImages();
    BodyWithAddedImages = GetBodyWithAddedImages();
    BodyWithAllImagesChanged = GetBodyWithAllImagesChanged();
    ExpectedNewFileName = GetExpectedNewFileName();
    ExpectedLink = GetExpectedLink();
    DefaultCachedFileNamesList = GetDefaultCachedFileNamesList();
    EmptyCachedFileNamesList = GetEmptyCachedFileNamesList();
    BodyWithUnusedCachedImages = GetBodyWithUnusedCachedImages();
    CachedFileNamesListWithUnusedImages = GetCachedFileNamesListWithUnusedImages();
    ListOfImagesExpectedToDelete = GetListOfImagesExpectedToDelete();
    BodyWithUsedAndUnusedImages = GetBodyWithUsedAndUnusedImages();
    CachedFileNamesListWithAllUnusedImages = GetCachedFileNamesListWithAllUnusedImages();
    
        MockImageService = new ImageService(
            MockConfiguration.Object, 
            MockImageRepository.Object,
            MockMemoryCache.Object,
            MockLoggerManager.Object);
    }
    
    public Mock<IConfiguration> MockConfiguration { get; }
    public Mock<IImageRepository> MockImageRepository { get; }
    public Mock<IMemoryCache> MockMemoryCache { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    public ImageService MockImageService { get; }
    public Mock<IFormFile> MockFormFile { get; }
    
    public string BodyWithNoImages { get; }
    public string BodyWithThreeInnerImages { get; }
    public string BodyWithThreeInnerAndOuterImages { get; }
    public string TrimmedBodyWithThreeInnerAndOuterImages { get; }
    public string BodyWithThreeInnerAndOuterWithQueryImages { get; }
    public string TrimmedBodyWithThreeInnerAndOuterWithQueryImages { get; }    
    public string BodyWithThreeInnerAndOuterWithQueryAndAttributesImages { get; }
    public string TrimmedBodyWithThreeInnerAndOuterWithQueryAndAttributesImages { get; }
    public string ChangedBodyWithTheSameImages { get; }
    public string BodyWithAddedImages { get; }
    public string BodyWithAllImagesChanged { get; }
    public string ExpectedNewFileName { get; }
    public string ExpectedLink { get; }
    public object DefaultCachedFileNamesList;
    public object EmptyCachedFileNamesList;
    public string BodyWithUnusedCachedImages { get; }
    public object CachedFileNamesListWithUnusedImages; 
    public object CachedFileNamesListWithAllUnusedImages; 
    public List<string> ListOfImagesExpectedToDelete { get; }
    public string BodyWithUsedAndUnusedImages { get; }

    private string GetBodyWithNoImages()
    {
        var body = "<ul><li><font face=\"Times New Roman\">hello</font></li><li><font face=\"Times New Roman\">for</font></li><li><u><font face=\"Times New Roman\">test example</font></u></li></ul>";
        return body;
    }

    private string GetBodyWithThreeInnerImages()
    {
        var body =
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul>";
        return body;
    }

    private string GetBodyWithThreeInnerAndOuterImages()
    {
        var body =
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\" alt=\"Clipping Magic: Remove Background From Image\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        return body;
    }

    private string GetTrimmedBodyWithThreeInnerAndOuterImages()
    {
        var trimmedBody = 
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        return trimmedBody;
    }

    private string GetBodyWithThreeInnerAndOuterWithQueryImages()
    {
        var body =
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg?name=ferret&color=purple\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg?param=hello&height=123\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg?width=17&color-name=purple\" alt=\"Clipping Magic: Remove Background From Image\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        return body;
    }

    private string GetTrimmedBodyWithThreeInnerAndOuterWithQueryImages()
    {
        var trimmedBody = 
            "<ul><li><font face=\"Times New Roman\">helo</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\">&#160;example</font></u></li><ul><ul><li><font face=\"Times New Roman\"><u><i>Let&#160;</i></u></font><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"></li><li><font face=\"Times New Roman\"><u><i>They<br></i></u></font></li></ul><li><u><i>Be&#160;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\">&#160;Or Not To Be</i></u><br></li></ul><li><font face=\"Times New Roman\"><i><u>With</u></i></font></li><ul><ul><ul><ul><ul><ul><li><font face=\"Times New Roman\"><i><u>Images&#160;</u></i></font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\"></li></ul></ul></ul></ul></ul></ul></ul>";
        return trimmedBody;
    }

    private string GetBodyWithThreeInnerAndOuterWithQueryAndAttributesImages()
    {
        var body =
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img class=\"image-wrapper\" src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg?name=ferret&color=purple\" alt=\"What Is Image Processing: Overview, Applications, Benefits, and Who Should Learn It [2022 Edition]\"><img class=\"hello-image-class\" src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg?param=hello&height=123\" alt=\"Premium Photo | Astronaut in outer open space over the planet earth.stars provide the background.erforming a space above planet earth.sunrise,sunset.our home. iss.elements of this image furnished by nasa.\"><img style=\"width: 40px\" src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg?width=17&color-name=purple\" alt=\"Clipping Magic: Remove Background From Image\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";
        return body;
    }

    private string GetTrimmedBodyWithThreeInnerAndOuterWithQueryAndAttributesImages()
    {
        var trimmedBody = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"https://img.freepik.com/premium-photo/astronaut-outer-open-space-planet-earth-stars-provide-background-erforming-space-planet-earth-sunrise-sunset-our-home-iss-elements-this-image-furnished-by-nasa_150455-16829.jpg\"><img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">";
        return trimmedBody;
    }

    private string GetChangedBodyWithTheSameImages()
    {
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for<li><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></ul>";
        return newBody;
    }

    private string GetBodyWithAddedImages()
    {
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e477c2e4-ab43-4de6-812a-6f34afd47ec7.png\"></li><li><font face=\"Times New Roman\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/8c2f8f20-2f89-4b8f-b3ff-7543718e38d0.png\">for</font></li><li><u><font face=\"Times New Roman\">test&#160;<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c8d78ab6-99a6-46cc-b157-8be1f0575272.jpg\">&#160;example</font></u></li></ul><ol><li><font face=\"Arial\"><i><strike>hello <img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/14f462e3-3700-44dd-b3d6-a7b685508e37.png\"></strike></i></font></li><ol><ol><li><font face=\"Arial\"><i><u>Pre&#160;</u></i></font><i><strike><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c0b59c6b-432e-4f22-be04-0006f8747136.png\"></strike></i></li></ol></ol></ol><i><p><i>Tsts&#160;</i></p></i>";
        return newBody;
    }

    private string GetBodyWithAllImagesChanged()
    {
        var newBody = 
            "<ul><li><font face=\"Times New Roman\">hello</font><li><li><font face=\"Times New Roman>for</font></li><li><u><font face=\"Times New Roman\">test&#160;&#160;example</font></u></li></ul><ol><li><font face=\"Arial\"><i><strike>hello <img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/14f462e3-3700-44dd-b3d6-a7b685508e37.png\"></strike></i></font></li><ol><ol><li><font face=\"Arial\"><i><u>Pre&#160;</u></i></font><i><strike><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/c0b59c6b-432e-4f22-be04-0006f8747136.png\"></strike></i></li></ol></ol></ol><i><p><i>Tsts&#160;</i></p></i>";
        return newBody;
    }

    private string GetExpectedNewFileName()
    {
        var expectedNewFileName = "439e7759-7de1-42e8-ad6d-8bed3723b676.png";
        return expectedNewFileName;
    }

    private string GetExpectedLink()
    {
        var expectedLink = $"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/{GetExpectedNewFileName()}";
        return expectedLink;
    }

    private object GetDefaultCachedFileNamesList()
    {
        var currentFileNames = new List<string>()
        {
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp"
        } as object;
        return currentFileNames;
    }

    private object GetEmptyCachedFileNamesList()
    {
        var o = null as object;
        return o;
    }

    private string GetBodyWithUnusedCachedImages()
    {
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";
        return body;
    }

    private object GetCachedFileNamesListWithUnusedImages()
    {
        var currentFileNames = new List<string>()
        {
            //Used
            "492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png",
            "0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg",
            "45025163-0b68-4ea0-9fd6-e74a5e49a894.png", 
            "e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp",
            //Unused
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;
        return currentFileNames;
    }

    private List<string> GetListOfImagesExpectedToDelete()
    {
        var expectedDeletedImages = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png",
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        };
        return expectedDeletedImages;
    }

    private string GetBodyWithUsedAndUnusedImages()
    {
        var body = 
            "<img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/492a5cbb-4998-4bc4-94f6-6f5c97194f7c.png\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/0dce4f04-58a9-4c61-94a8-c4ced4ead76d.jpg\"><img src=\"https://www.simplilearn.com/ice9/free_resources_article_thumb/what_is_image_Processing.jpg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/e43f8e7a-b99a-4b53-811d-59bcdbe502aa.webp\">;<img src=\"https://d5nunyagcicgy.cloudfront.net/external_assets/hero_examples/hair_beach_v391182663/original.jpeg\"><img src=\"http://127.0.0.1:10000/devstoreaccount1/vet-clinic/articles/45025163-0b68-4ea0-9fd6-e74a5e49a894.png\">";
        return body;
    }

    private object GetCachedFileNamesListWithAllUnusedImages()
    {
        var currentFileNames = new List<string>()
        {
            "378b330c-5fe7-4ee2-aa4e-5668473bce5c.png",
            "aa84fb1c-f0e8-4925-a98e-94c67849605e.jpg",
            "6f850056-9032-4ecf-9e2c-b7f531ecdd62.png", 
            "79938366-258d-4f0f-8dd2-61efc84e19b2.webp"
        } as object;
        return currentFileNames;
    }
}