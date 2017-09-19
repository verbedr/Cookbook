# migration commands
## enable migrations
enable-migrations -StartUpProjectName Repository -ProjectName Repository -ContextTypeName Cookbook.Repository.CookbookDbContext

## consolidate domain changes into a migration script
add-migration -StartUpProjectName Repository -ProjectName Repository -ConnectionStringName Cookbook.Repository.CookbookDbContext

## apply migration
update-database -StartUpProjectName Repository -ProjectName Repository  -ConnectionStringName Cookbook.Repository.CookbookDbContext
