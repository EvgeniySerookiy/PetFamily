// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using PetFamily.Application.Messaging;
// using PetFamily.Application.Photos;
// using PetFamily.Infrastructure.DbContexts;
// using PetFamily.Infrastructure.Options;
//
// namespace PetFamily.Infrastructure.BackgroundServices;
//
// public class SoftDeleteCleanupService : BackgroundService
// {
//     private const string BUCKET_NAME = "photos";
//
//     private readonly IServiceScopeFactory _serviceScopeFactory;
//     private readonly ILogger<SoftDeleteCleanupService> _logger;
//     private readonly IMessageQueue<IEnumerable<PhotoInfo>> _messageQueue;
//     private readonly SoftDeleteOptions _softDeleteOptions;
//
//     public SoftDeleteCleanupService(
//         IServiceScopeFactory serviceScopeFactory,
//         IOptions<SoftDeleteOptions> options,
//         ILogger<SoftDeleteCleanupService> logger,
//         IMessageQueue<IEnumerable<PhotoInfo>> messageQueue)
//     {
//         _serviceScopeFactory = serviceScopeFactory;
//         _softDeleteOptions = options.Value;
//         _logger = logger;
//         _messageQueue = messageQueue;
//     }
//
//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         _logger.LogInformation("SoftDeleteCleanupService is starting.");
//
//         using PeriodicTimer timer = new(_softDeleteOptions.CheckPeriod);
//
//         while (stoppingToken.IsCancellationRequested == false &&
//                await timer.WaitForNextTickAsync(stoppingToken))
//             try
//             {
//                 await CleanupDeletedVolunteersAsync(stoppingToken);
//                 await CleanupDeletedPetsAsync(stoppingToken);
//
//                 _logger.LogInformation("Soft delete cleanup completed successfully");
//             }
//             catch (Exception exception)
//             {
//                 _logger.LogError(exception, exception.Message);
//             }
//     }
//
//     private async Task CleanupDeletedVolunteersAsync(CancellationToken cancellationToken)
//     {
//         var writeDbContext = GetScopedWriteDbContext();
//         
//         var deletedVolunteers = writeDbContext.Volunteers
//             .Where(v => v.IsDeleted &&
//                         DateTime.UtcNow - v.DeletionDate >= _softDeleteOptions.TimeToRestore);
//
//         writeDbContext.Volunteers.RemoveRange(deletedVolunteers);
//
//         await writeDbContext.SaveChangesAsync(cancellationToken);
//     }
//
//     private async Task CleanupDeletedPetsAsync(CancellationToken cancellationToken)
//     {
//         var writeDbContext = GetScopedWriteDbContext();
//
//         var deletedPets = GetScopedWriteDbContext().Pets
//             .Where(v => v.IsDeleted &&
//                         DateTime.UtcNow - v.DeletionDate >= _softDeleteOptions.TimeToRestore);
//
//         var photosToDelete = new List<PhotoInfo>();
//         
//          foreach (var pet in deletedPets)
//          {
//              if (pet.PetPhotos != null)
//              {
//                  foreach (var photo in pet.PetPhotos)
//                  {
//                      var photoInfo = new PhotoInfo(photo.PathToStorage, BUCKET_NAME);
//                      photosToDelete.Add(photoInfo);
//                  }
//              }
//          }
//
//          writeDbContext.Pets.RemoveRange(deletedPets);
//         
//         if (photosToDelete.Any())
//         {
//             await _messageQueue.WriteAsync(photosToDelete, cancellationToken);
//         }
//
//         await writeDbContext.SaveChangesAsync(cancellationToken);
//     }
//
//     private WriteDbContext GetScopedWriteDbContext()
//     {
//         using var scope = _serviceScopeFactory.CreateScope();
//         var writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
//         return writeDbContext;
//     }
// }