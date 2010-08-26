@echo off
cls
start /b "C:\program files\Microsoft Visual Studio 9\Common7\IDE\devenv.exe" "NHibernate Empty\NHibernateEmptyDemo.sln" /Edit
start /b "C:\program files\Microsoft Visual Studio 9\Common7\IDE\devenv.exe" "NHibernate Fluent\NHibernateFluentDemo.sln" /Edit
start /b "C:\program files\Microsoft Visual Studio 9\Common7\IDE\devenv.exe" "NHibernate\NHibernateWorkingDemo.sln" /Edit
start /d "C:\Apps\Development_Utils\NHibernate Profiler\" NHProf.exe
start NHibernate.pptx
