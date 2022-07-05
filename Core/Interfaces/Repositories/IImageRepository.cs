using System.Drawing;

namespace Core.Interfaces.Repositories;

public interface IImageRepository
{
    /// <summary>
    /// Uploads image to the Azure blob storage
    /// </summary>
    /// <param name="image">Image to upload</param>
    /// <param name="folder">Folder on your blob where the image will be uploaded. For example - <b>articles</b> , without slash</param>
    /// <param name="imageFormat">Image format, like <b>png</b></param>
    /// <param name="fileName">Optionally name of the file. If not provided - a file will be named with new guid. Note that <b>fileName</b> be unique</param>
    /// <example>var resultName = await UploadFromImageAsync(<b><br></br>image: readImage, <br></br>folder: "articles", <br></br>imageFormat: "png", <br></br>fileName: "{firstName}-{email}"</b>)</example>
    /// <returns>A path to the file, including folder, file name and format: <b>$"{folder}/{fileName}.{imageFormat}"</b></returns>
    public Task<string> UploadFromImageAsync(
        Image image, 
        string folder,
        string imageFormat,
        string fileName = "");
    
    /// <summary>
    /// Converts base64 string to an image and uploads it to the Azure blob storage
    /// </summary>
    /// <param name="base64">Base 64 string to convert and than upload</param>
    /// <param name="folder">Folder on your blob where the image will be uploaded. For example - <b>articles</b> , without slash</param>
    /// <param name="imageFormat">Image format, like <b>png</b></param>
    /// <param name="fileName">Optionally name of the file. If not provided - a file will be named with new guid. Note that <b>fileName</b> be unique</param>
    /// <example>var resultName = await UploadFromBase64Async(<b><br></br>base64: "*very large string here*", <br></br>folder: "articles", <br></br>imageFormat: "png", <br></br>fileName: "{firstName}-{email}"</b>)</example>
    /// <returns>A path to the file, including folder, file name and format: <b>$"{folder}/{fileName}.{imageFormat}"</b></returns>
    public Task<string> UploadFromBase64Async(
        string base64,
        string folder,
        string imageFormat,
        string fileName = "");

    /// <summary>
    /// Deletes a file from the folder of Azure blob storage
    /// </summary>
    /// <param name="imageName">Image name to delete</param>
    /// <param name="folder">Folder on your blob from where the image will be deleted. For example - <b>articles</b> , without slash</param>
    /// <example>the file will be deleted from articles/0f8fad5b-d9cb-469f-a165-70867728950e.png: <br></br> await DeleteAsync(<b><br></br>imageName: "0f8fad5b-d9cb-469f-a165-70867728950e.png", <br></br>folder: "articles"</b>)</example>
    public Task DeleteAsync(string imageName, string folder);
    
    /// <summary>
    /// Deletes a file from the Azure blob storage
    /// </summary>
    /// <param name="path">Path of the file to delete. Like "articles/0f8fad5b-d9cb-469f-a165-70867728950e.png"</param>
    /// <example>the file will be deleted from articles/0f8fad5b-d9cb-469f-a165-70867728950e.png: <br></br> await DeleteAsync(<b><br></br>path: "articles/0f8fad5b-d9cb-469f-a165-70867728950e.png"</b>)</example>
    public Task DeleteAsync(string path);

}