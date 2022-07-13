package test.api

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import io.gatling.jdbc.Predef._
import scala.concurrent.duration._
import scala.util.Random

class ApiBehaviorLocal extends Simulation {

  object ApiGet {

	val healthCheck = exec(http("CheckWeatherForecast")
	    .get("/api/v1/WeatherForecast")
		.header("Content-Type", "application/json"))

  }

  val httpProtocol = http
    .baseUrl("http://localhost:5001")
    .acceptHeader("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8")
    .doNotTrackHeader("1")
    .acceptLanguageHeader("en-US,en;q=0.5")
    .acceptEncodingHeader("gzip, deflate")
    .userAgentHeader("Mozilla/5.0 (Macintosh; Intel Mac OS X 10.8; rv:16.0) Gecko/20100101 Firefox/16.0")

  // Now, we can write the scenario as a composition

  val initScenario = scenario("init scenario")
					.exec(ApiGet.healthCheck)
					.exitHereIfFailed


 setUp(
 	  initScenario.inject(
 		  atOnceUsers(10),
		  nothingFor(5 seconds),
		  rampUsers(50) during (1 minutes),
		  nothingFor(5 seconds),
		  atOnceUsers(200),
		  nothingFor(5 seconds),
		  atOnceUsers(500),
		  nothingFor(5 seconds),
		  atOnceUsers(1000)
 		  ).protocols(httpProtocol)
 	  )
}
