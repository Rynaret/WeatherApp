# WeatherApp

**GismeteoGrabber** parses gismeteo main page, take popular cities from it and load ten days forecast for each city.

**Wcf.ForecastService** consumes requests from WPF App. WCF goes to MySql DB and responses to the consumer.

**Grpc.ForecastService** Microsoft recommends to use gRPC against Wcf. So there is a try. (grpc branch)

In the project, I have touched new WPF Core 3.0 with WCF integration.

## How to run

The app uses *MySql*. There is a file `weather 20190818 1207.sql` in root folder. You can create table `weather` and restore data by the file.

To run *MySql* instance you can use `docker-compose.yml`. It ups *MySq*l on *127.0.0.1:3306* with default credentials `MYSQL_USER: "user"` and `MYSQL_PASSWORD: "password"`, which can be changed.

Maybe in future I will adjust `docker-compose.yml`. I going to add `GismeteoGrabber` and `Grpc.ForecastService` to it.
