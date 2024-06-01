using DataAccess.DAO.DAOIntefaces;
using DataAccess.Models;
using DataAccess.Repositories.RepositoryInterfaces;
using Dapper;

namespace DataAccess.Repositories
{
    public class DigitalItemRepository : IDigitalItemRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public DigitalItemRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _connectionFactory = databaseConnectionFactory;
        }

        public async Task<DigitalItem?> GetDigitalItem(int id)
        {
            string sql = "SELECT di.*, dia.Name " +
                "FROM DigitalItem di " +
                "LEFT JOIN DigitalItemAuthor dia ON di.Id = dia.DigitalItemId " +
                "WHERE Id = @Id";

            List<string> authors = new();
            using (var connection = _connectionFactory.CreateConnection())
            {
                var digitalItem = (await connection.QueryAsync<DigitalItem, string,
                   int?, Resolution?, string?, DigitalItem>(sql,
                   map: (digitalItem, type, length, resolution, author) =>
                   {
                       switch (type ?? "")
                       {
                           case "Text":
                               digitalItem = new Text(digitalItem);
                               break;
                           case "Video":
                               digitalItem = new Video(digitalItem, resolution ?? new(), length ?? 0);
                               break;
                           case "Audio":
                               digitalItem = new Audio(digitalItem, length ?? 0);
                               break;
                           case "Image":
                               digitalItem = new Image(digitalItem, resolution ?? new());
                               break;
                           default:
                               break;
                       }
                       authors.Add(author ?? "");
                       digitalItem.Authors = authors;
                       return digitalItem;
                   },
                   splitOn: "DigitalItemType, Length, ResolutionWidth, Name",
                   param: new
                   {
                       id
                   })).AsQueryable().FirstOrDefault();
                return digitalItem;
            }
        }

        public async Task<List<DigitalItem>> ListDigitalItems()
        {
            string sql = "SELECT di.*, dia.Name " +
                "FROM DigitalItem di " +
                "LEFT JOIN DigitalItemAuthor dia ON di.Id = dia.DigitalItemId";

            Dictionary<int, List<string>> digitalItemAuthorPairs = new();

            using (var connection = _connectionFactory.CreateConnection())
            {
                var digitalItem = (await connection.QueryAsync<DigitalItem, string,
                    int?, Resolution?, string?, DigitalItem>(sql,
                    map: (digitalItem, type, length, resolution, author) =>
                    {
                        switch (type)
                        {
                            case "Text":
                                digitalItem = new Text(digitalItem);
                                break;
                            case "Video":
                                digitalItem = new Video(digitalItem, resolution ?? new(), length ?? 0);
                                break;
                            case "Audio":
                                digitalItem = new Audio(digitalItem, length ?? 0);
                                break;
                            case "Image":
                                digitalItem = new Image(digitalItem, resolution ?? new());
                                break;
                            default:
                                break;
                        }
                        digitalItemAuthorPairs.TryAdd(digitalItem.Id, new List<string>());
                        var authors = digitalItemAuthorPairs[digitalItem.Id];
                        authors.Add(author);
                        digitalItem.Authors = authors;
                        return digitalItem;
                    },
                    splitOn: "DigitalItemType, Length, ResolutionWidth, Name"
                    )).AsQueryable().DistinctBy(digitalItem => digitalItem.Id).ToList();
                return digitalItem;
            }
        }

        public async Task<Audio> CreateAudio(Audio audio)
        {
            string sqlAudio = "INSERT INTO DigitalItem (Id, Name, Size, Format, DigitalItemType, Length) " +
                "VALUES (@Id, @Name, @Size, @Format, @DigitalItemType, @Length)";

            string sqlAuthor = "INSERT INTO DigitalItemAuthor (DigitalItemId, Name)" +
                " VALUES (@DigitalItemId, @Name)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlAudio, new
                    {
                        audio.Id,
                        audio.Name,
                        audio.Size,
                        audio.Format,
                        DigitalItemType = "Audio",
                        audio.Length
                    }, transaction);
                    foreach (var author in audio.Authors ?? [])
                    {
                        await connection.ExecuteAsync(sqlAuthor, new
                        {
                            DigitalItemId = audio.Id,
                            Name = author
                        }, transaction);
                    }
                    transaction.Commit();
                }
            }
            return audio;
        }

        public async Task<Image> CreateImage(Image image)
        {
            string sqlImage = "INSERT INTO DigitalItem (Id, Name, Size, Format, DigitalItemType, " +
                "ResolutionWidth, ResolutionHeight) VALUES (@Id, @Name, @Size, @Format, @DigitalItemType, " +
                "@ResolutionWidth, @ResolutionHeight)";

            string sqlAuthor = "INSERT INTO DigitalItemAuthor (DigitalItemId, Name)" +
                " VALUES (@DigitalItemId, @Name)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlImage, new
                    {
                        image.Id,
                        image.Name,
                        image.Size,
                        image.Format,
                        DigitalItemType = "Image",
                        ResolutionWidth = image.Resolution?.Width,
                        ResolutionHeight = image.Resolution?.Height
                    }, transaction);
                    foreach (var author in image.Authors ?? [])
                    {
                        await connection.ExecuteAsync(sqlAuthor, new
                        {
                            DigitalItemId = image.Id,
                            Name = author
                        }, transaction);
                    }
                    transaction.Commit();
                }
            }
            return image;
        }

        public async Task<Text> CreateText(Text text)
        {
            string sqlText = "INSERT INTO DigitalItem (Id, Name, Size, Format, DigitalItemType) " +
                "VALUES (@Id, @Name, @Size, @Format, @DigitalItemType)";

            string sqlAuthor = "INSERT INTO DigitalItemAuthor (DigitalItemId, Name)" +
                " VALUES (@DigitalItemId, @Name)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlText, new
                    {
                        text.Id,
                        text.Name,
                        text.Size,
                        text.Format,
                        DigitalItemType = "Text"
                    }, transaction);
                    foreach (var author in text.Authors ?? [])
                    {
                        await connection.ExecuteAsync(sqlAuthor, new
                        {
                            DigitalItemId = text.Id,
                            Name = author
                        }, transaction);
                    }
                    transaction.Commit();
                }
            }
            return text;
        }

        public async Task<Video> CreateVideo(Video video)
        {
            string sqlVideo = "INSERT INTO DigitalItem (Id, Name, Size, Format, DigitalItemType, Length, " +
                "ResolutionWidth, ResolutionHeight) VALUES (@Id, @Name, @Size, @Format, @DigitalItemType, @Length, " +
                "@ResolutionWidth, @ResolutionHeight)";

            string sqlAuthor = "INSERT INTO DigitalItemAuthor (DigitalItemId, Name)" +
                " VALUES (@DigitalItemId, @Name)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(sqlVideo, new
                    {
                        video.Id,
                        video.Name,
                        video.Size,
                        video.Format,
                        DigitalItemType = "Video",
                        video.Length,
                        ResolutionWidth = video.Resolution?.Width,
                        ResolutionHeight = video.Resolution?.Height
                    }, transaction);
                    foreach (var author in video.Authors ?? [])
                    {
                        await connection.ExecuteAsync(sqlAuthor, new
                        {
                            DigitalItemId = video.Id,
                            Name = author
                        }, transaction);
                    }
                    transaction.Commit();
                }
            }
            return video;
        }

        public async Task<DigitalItem> UpdateDigitalItem(DigitalItem digitalItem)
        {
            string sql = "UPDATE DigitalItem SET Name = @Name, Size = @Size, Format = @Format " +
                "WHERE Id = @Id";

            using (var connection = _connectionFactory.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, digitalItem);
            }
            return digitalItem;
        }

        public async Task<bool> DeleteDigitalItem(int id)
        {
            string sql = "DELETE FROM DigitalItem WHERE Id = @Id";
            using (var connection = _connectionFactory.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    id
                });

                return rowsAffected == 1;
            }
        }
    }

}
