# MicroORM


Micro ORM is a library that maps between objects and database queries.
This micro ORM not only maps the result of a query, but also provides a complete C# expression assembler, thus eliminating the explicit sql in the code.
It cannot be classified as a full ORM because it does not support query buffers, query assembly buffers, joins, on-demand subqueries.

The library will be functional after the basic testing phase, first topic of NEXT FEATURES.


Next features:

- Test current implementations
- Auto create tables, fields and changes
- Implementation of keywords: order by, limit, offset
- Implementation of the functions: Length, Contains, Count, Max, Min
- Implementation of table joins (only 3 levels will be available)
- Performance tests
