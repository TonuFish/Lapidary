# based on https://github.com/jupyter/docker-stacks/blob/master/base-notebook/Dockerfile
FROM ubuntu:25.10

USER root

# Configure environment
ENV SHELL=/bin/bash \
    GS_USER=gemstone \
    GS_UID=1001 \
    GS_GID=100 \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    LANGUAGE=en_US.UTF-8 \
    GEMSTONE_GLOBAL_DIR=/opt/gemstone
ENV HOME=/home/$GS_USER \
    GEMSTONE=$GEMSTONE_GLOBAL_DIR/product
ENV	PATH=$GEMSTONE/bin:$PATH \
    GEMSTONE_EXE_CONF=$GEMSTONE_GLOBAL_DIR/conf \
    GEMSTONE_SYS_CONF=$GEMSTONE_GLOBAL_DIR/conf \
    GEMSTONE_NRS_ALL=#netldi:gs64ldi#dir:$GEMSTONE_GLOBAL_DIR/#log:$GEMSTONE_GLOBAL_DIR/log/%N_%P.log \
    GEMSTONE_LOGDIR=$GEMSTONE_GLOBAL_DIR/log \
    GEMSTONE_LOG=$GEMSTONE_GLOBAL_DIR/log/gs64stone.log \
    GS_FORCE_CLEAN_LOG_FILE_DELETE=true

# Install SSH
RUN apt update && \
    apt install openssh-server sudo -y

RUN groupadd wheel -g 11 && \
    echo "auth required pam_wheel.so use_uid" >> /etc/pam.d/su && \
    useradd -m -s /bin/bash -g root -G sudo -N -u $GS_UID $GS_USER && \
    mkdir -p $GEMSTONE && \
    chown $GS_USER:$GS_GID $GEMSTONE_GLOBAL_DIR && \
    chmod g+w /etc/passwd

# Extra SSH configuration
RUN echo "${GS_USER}:gemstone" | chpasswd
RUN echo "${GS_USER} ALL=(ALL) NOPASSWD: ALL" | tee -a /etc/sudoers
RUN service ssh start
EXPOSE 22

USER $GS_UID
WORKDIR $GEMSTONE_GLOBAL_DIR

COPY --chown=gemstone:users gemstone/product $GEMSTONE_GLOBAL_DIR/product
COPY --chown=gemstone:users gemstone/data $GEMSTONE_GLOBAL_DIR/data
COPY --chown=gemstone:users gemstone/conf $GEMSTONE_GLOBAL_DIR/conf
COPY --chown=gemstone:users gemstone/locks $GEMSTONE_GLOBAL_DIR/locks
COPY --chown=gemstone:users gemstone/log $GEMSTONE_GLOBAL_DIR/log
COPY --chown=gemstone:users gemstone/gemstone.sh $GEMSTONE_GLOBAL_DIR/gemstone.sh
COPY --chown=gemstone:users gemstone/Classes /srv/Lapidary/Classes
COPY --chown=gemstone:users gemstone/seed.topaz $GEMSTONE_GLOBAL_DIR/seed.topaz

EXPOSE 50377
CMD ["./gemstone.sh"]
