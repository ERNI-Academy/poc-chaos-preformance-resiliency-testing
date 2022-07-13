param([string]$CloudServerRepository = '',[string]$user = '',[string]$password = '')

Write-Host " --------------- Connecting to cloud ---------------"
docker login --username=$user --password=$password $CloudServerRepository

Write-Host " ---------------Build images ---------------"
docker build -t microservices.generatingchaos.gatling -f Dockerfile .

Write-Host " ---------------Tag images ---------------"
docker tag  microservices.generatingchaos.gatling $CloudServerRepository/microservices.generatingchaos.gatling

Write-Host " --------------- Push images ---------------"
docker push $CloudServerRepository/microservices.generatingchaos.gatling

Write-Host " --------------- Run gatling image ---------------"
docker run -it microservices.generatingchaos.gatling "gatling.sh", "-sf", "user-files", "-s", "test.api.ApiBehavior1"