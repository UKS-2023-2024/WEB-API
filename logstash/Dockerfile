FROM docker.elastic.co/logstash/logstash:8.7.0

# copy the postgres jdbc driver to the external library folder of logstash
COPY ./bin/ /usr/share/logstash/logstash-core/lib/jars/

# copy configurations
COPY logstash/pipeline/ /usr/share/logstash/pipeline/
COPY logstash/config/ /usr/share/logstash/config/


