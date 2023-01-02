
## step1: Build an Image for the python application
```
docker build -t python_sinwave .
```
## step2: Run a container from the image exposed on port 1996 on detach mode
```
docker run -d --name python-sinwave-container -p 1996:1994 python_sinwave
```
