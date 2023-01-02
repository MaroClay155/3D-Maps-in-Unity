FROM python:3
WORKDIR /usr/app/src

COPY requirements.txt ./
RUN apt update
RUN apt install python3 -y
RUN pip install --no-cache-dir --upgrade pip && \
    pip install --no-cache-dir nibabel pydicom matplotlib pillow med2image


COPY . .
EXPOSE 1994

CMD [ "python3", "./sinwave.py" ]
