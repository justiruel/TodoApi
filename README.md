# TodoApi
## Get All Todo’s
- /GET http://localhost:5000/api/todo

## Get Specific Todo
- /GET http://localhost:5000/api/todo/get-by-name/{title}

## Get Incoming ToDo (for today/next day/current week)
- /GET http://localhost:5000/api/todo/incoming/{id}
- note
```
ID =>
           1 = Today
           2 = tomorrow
           3 = current week
```

## Create Todo
- /POST http://localhost:5000/api/todo
- param
```
{
	"Title":"Task A",
	"ExpirateDate":"2020-04-15",
	"Description":"Doing Task A",
	"PercentageComplete":0
}
```

## Update Todo
- /PUT http://localhost:5000/api/todo/{id}
- param
```
{
	"Title":"Task A",
	"ExpirateDate":"2020-04-15",
	"Description":"Doing Task A",
	"PercentageComplete":0
}
``` 
- note => id == id todo

## Set Todo percent complete
- /GET http://localhost:5000/api/todo/set-percentage/{id}/{percentage}
- note
```
id =  todo id
percentage =  percentage complete 
```

## Delete Todo
- /DELETE http://localhost:5000/api/todo/{id}
- note => id == id todo

## Mark Todo as Done
- /GET http://localhost:5000/api/todo/mark-as-done/{id}
- note => id == id todo

## Unachieved plan
- adding swagger
- unit testing
- date validation using regex checking
- using docker
