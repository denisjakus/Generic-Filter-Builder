# Welcome to Generic-Filter-Builder!

Hi! This is my first Nuget package. I think you will find it pretty useful when creating dynamically generated filters. 

**GenericFilterBuilder** is a generic filter builder class. Each **YourDomainClass** can be filtered by its properties if they are provided within **filterValue**.

FilterValue param is an array of **FilterValueItem** class serialized to Json into string.
> **Sample filter:** 
```
[
{"filterKey":"yourClassPropertyName","filterValue":propertyValue},
{"filterKey":"yourClassPropertyName2","filterValue":propertyValue2},
...]
```

>**Disclaimer**: 
>These examples are only intended to provide you enough information on how to use GenericFilterBuilder functionality 

# WebAPI - Example

Have in your **WebApi controller** or **BLL service layer** something like this one:
```
public async Task<YourDomainClassDto> GetYourWebApiUrlMethodName(string filterValue = "", string sortBy = "", OrderDirection orderDirection = OrderDirection.Desc,
	int pageNumber = 0, int pageSize = 10)
{
		// this is where we build our generic filter class
		var filter = new GenericFilterBuilder<YourDomainClass>()
		.AddOrFilters(filterValue)
		.Build();
		
	/// IGenericRepository interface is also provided so you can implement it within your Repository classs
	var filteredData = await this.UnitOfWork.YourDomainClassRepository.GetAllPagedAndFilteredAsync(filter, pageNumber, pageSize, sortBy, orderDirection);

	// only for demo purpose 

	return filteredData;
}
```


# Repository - example

In this example, some custom wrapper  SqlDapper was used, so you can ignore that part.
Main goal here is to send dynamically built filter to **Queryable WHERE()** 
```
        public async Task<IReadOnlyList<YourDomainClass>> GetAllPagedAndFilteredAsync(Func<YourDomainClass, bool> filter = null, int? page = null, int? recordsPerPage = null, string orderBy = null, OrderDirection orderDirection = OrderDirection.Desc)
        {
			var dbData = await QueryDapperHelper.QueryAsync(...); // only for demo purpose
				
            if (filter != null)
                dbData = dbData.AsQueryable().Where(filter); // i.e.: if EF used: dbSet.Where(filter)

            if (!string.IsNullOrEmpty(orderBy))
            {
                dbData = dbData.AsQueryable().StringOrderBy(orderBy, orderDirection);
            }

            if (page.HasValue && recordsPerPage.HasValue)
                dbData = dbData.Skip(page.Value * recordsPerPage.Value).Take(recordsPerPage.Value);

            return dbData.ToList<YourDomainClass>();
        }
```
Hopefully this is clear enough. If any questions, send me an email.
Cheers.
