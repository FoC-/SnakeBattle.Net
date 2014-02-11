if not exist data goto nodatadir
:returnnodatadir
mongod.exe --dbpath data
exit

:nodatadir
md data
goto returnnodatadir
