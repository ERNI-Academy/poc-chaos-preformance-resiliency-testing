# Gatling is a highly capable load testing tool.
#
# Documentation: https://gatling.io/docs/3.1/
# Cheat sheet: https://gatling.io/docs/3.1/cheat-sheet/

FROM openjdk:8-jdk-alpine

RUN apk --no-cache add curl

# working directory for gatling
WORKDIR /opt

# gating version
#ENV GATLING_VERSION 3.1.3
#ENV GATLING_VERSION 3.2.0
ENV GATLING_VERSION 3.7.6

# create directory for gatling install
RUN mkdir -p gatling

# install gatling
RUN apk add --update wget bash libc6-compat && \
    mkdir -p /tmp/downloads && \
    wget -q -O /tmp/downloads/gatling-$GATLING_VERSION.zip \
    https://repo1.maven.org/maven2/io/gatling/highcharts/gatling-charts-highcharts-bundle/$GATLING_VERSION/gatling-charts-highcharts-bundle-$GATLING_VERSION-bundle.zip && \
    mkdir -p /tmp/archive && cd /tmp/archive && \
    unzip /tmp/downloads/gatling-$GATLING_VERSION.zip && \
    mv /tmp/archive/gatling-charts-highcharts-bundle-$GATLING_VERSION/* /opt/gatling/ && \
    rm -rf /tmp/* && \
    rm -rf /opt/gatling/user-files/simulations/* && \
    rm -rf /opt/gatling/user-files/resources/* 

# Copy simulation files
COPY simulationscripts/simulations /opt/gatling/user-files/simulations/

# Copy conf files
COPY conf /opt/gatling/conf/

# change context to gatling directorygatli
WORKDIR /opt/gatling

# set directories below to be mountable from host
VOLUME ["/opt/gatling/conf", "/opt/gatling/results", "/opt/gatling/user-files"]

# set environment variables
ENV PATH /opt/gatling/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
ENV GATLING_HOME /opt/gatling

# COMMENTED LINE!!!
#ENTRYPOINT ["gatling.sh", "-rd", "test.api.ApiBehaviorK8s"]