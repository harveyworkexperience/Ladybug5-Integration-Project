// Code from: https://stackoverflow.com/questions/15731924/udp-multi-client-server-basics
// Rewritten to use tabs over spaces.
// Code currently just echoes message back to client.
// The client can be a client written using C#.

// C UDP Server Libraries
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>
#include <netdb.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

// C++ Libraries for loading images
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>
#include <string>

#define BUFSIZE 	1024	// Size of message buffer (Basically the packet size for sending and receiving)
							// but specifically for communicating with the client rather than sending stuff

#define PACKET_SIZE	50000	// Size of packet to send (in bytes)

#define SLEEP_TIME 	50000 	// Time to wait after sending a packet (in microseconds)

// error - wrapper for perror
void error(const char* msg) {
	perror(msg);
	exit(1);
}

// Loads an image using the relative file path and returns a string
// (i.e. File location relative to compiled binary)
std::string LoadImage(std::string fname) {
	printf("Loading image at %s ...", fname.c_str());
	try {
		std::ifstream ifs(fname, std::ios::in | std::ios::binary); // input file
	    std::ostringstream oss; // output to string

	    int len;
	    char buf[1024];
	    while((len = ifs.readsome(buf, 1024)) > 0)
	        oss.write(buf, len);
		printf("SUCCESS!\n");	 
	    return oss.str(); // get string data out of stream
	}
	catch(const std::exception & ex) {
		printf("FAIL!\n");
		fprintf(stderr, "\t%s\n", ex.what());
		return "";
	}
}

// Loads multiple images and returns a vector of strings
// Note: 	pathname should be the common name of the images and it assumes the images are named like
//			this: imageXXXX.extension where XXXX is a number from (0001 - num_images)
std::vector<std::string> LoadMultipleImages(std::string pathname, int num_images, std::string extension) {
	printf("Loading %d images at path %sXXXX.%s ...", num_images, pathname.c_str(), extension.c_str());
    std::vector<std::string> v(num_images);
    for (int i = 0; i < num_images; i++)
    {
        std::string tempPathName = pathname;
        if (i < 9)
            tempPathName += "000" + std::to_string(i + 1) + "." + extension;
        else if (i < 99)
            tempPathName += "00" + std::to_string(i + 1) + "." + extension;
        else if (i < 999)
            tempPathName += "0" + std::to_string(i + 1) + "." + extension;
        else
            tempPathName += std::to_string(i + 1) + "." + extension;
        printf("          ");
        v[i] = LoadImage(tempPathName);
    }
    printf("Successfully loaded %d images at path %sXXXX.%s!", num_images, pathname.c_str(), extension.c_str());
    return v;
}

// A function for serving a client. Useful example for figuring out how to send images as packets.
void ServeClient(pid_t process, std::vector<std::string>& test_images, std::vector<std::string>& animation, 
				std::vector<std::string>& vr_video, char* hostaddrp, int portno,
				int& sockfd, struct sockaddr_in& clientaddr, socklen_t& clientlen) {
	int type = 1;
	int n;

	// Sending two images
	if (type == 0) {
		// Sending message
		printf("Sending image bytes to client at %s:%d\n", hostaddrp, portno);
		std::string message = "Sending image bytes!";
		n = sendto(sockfd, message.c_str(), message.length(), 0,
			(struct sockaddr*)&clientaddr, clientlen);
		if (n < 0) 
			error((const char*)"ERROR in sendto");

		while (true) {
			for (int i = 0; i < (int)test_images[0].length(); i += PACKET_SIZE) {
                std::string packet = test_images[0].substr(i, PACKET_SIZE);
                n = sendto(sockfd, packet.c_str(), packet.length(), 0,
					(struct sockaddr*)&clientaddr, clientlen);
				if (n < 0) 
					error((const char*)"ERROR in sendto");
                usleep(SLEEP_TIME);
            }
            for (int i = 0; i < (int)test_images[1].length(); i += PACKET_SIZE) {
                std::string packet = test_images[1].substr(i, PACKET_SIZE);
                n = sendto(sockfd, packet.c_str(), packet.length(), 0,
					(struct sockaddr*)&clientaddr, clientlen);
				if (n < 0) 
					error((const char*)"ERROR in sendto");
                usleep(SLEEP_TIME);
            }
		}
	}
	// Sending animation as images
	else if (type == 1) {
		// Sending message
		printf("Sending image bytes to client at %s:%d\n", hostaddrp, portno);
		std::string message = "Sending image bytes!";
		n = sendto(sockfd, message.c_str(), message.length(), 0,
			(struct sockaddr*)&clientaddr, clientlen);
		if (n < 0) 
			error((const char*)"ERROR in sendto");
		while (true) {
			for (int j=0; j<(int)animation.size(); j++) {
				for (int i = 0; i < (int)animation[j].length(); i += PACKET_SIZE) {
	                std::string packet = animation[j].substr(i, PACKET_SIZE);
	                n = sendto(sockfd, packet.c_str(), packet.length(), 0,
						(struct sockaddr*)&clientaddr, clientlen);
					if (n < 0) 
						error((const char*)"ERROR in sendto");
	                usleep(SLEEP_TIME);
	            }
			}
		}
	}
	else if (type == 2) {
		// Sending message
		printf("Sending image bytes to client at %s:%d\n", hostaddrp, portno);
		std::string message = "Sending image bytes!";
		n = sendto(sockfd, message.c_str(), message.length(), 0,
			(struct sockaddr*)&clientaddr, clientlen);
		if (n < 0) 
			error((const char*)"ERROR in sendto");
		while (true) {
			for (int j=0; j<(int)vr_video.size(); j++) {
				for (int i = 0; i < (int)vr_video[j].length(); i += PACKET_SIZE) {
	                std::string packet = vr_video[j].substr(i, PACKET_SIZE);
	                n = sendto(sockfd, packet.c_str(), packet.length(), 0,
						(struct sockaddr*)&clientaddr, clientlen);
					if (n < 0) 
						error((const char*)"ERROR in sendto");
	                usleep(SLEEP_TIME);
	            }
			}
		}
	}
	return;
}

int main(int argc, char** argv) {
	int sockfd;						// socket
	int portno;						// port to listen to
	socklen_t clientlen;			// byte size of client address
	struct sockaddr_in serveraddr;	// server's addr
	struct sockaddr_in clientaddr;	// client addr
	struct hostent* hostp;			// client host info
	char buf[BUFSIZE];				// message receive buffer
	char *hostaddrp;				// dotted decimal host addr string
	int optval;						// flag value for setsockopt
	int n;							// message byte size

	/*
	 * Relative Image File locations - C:\Work Experience\JPEG_Images\med_quality_vr\img"
	 */
	std::string vr_images 		= "../../../Work Experience/JPEG_Images/med_quality_vr/img";	// jpeg
	std::string test_image_1	= "../../../Work Experience/JPEG_Images/img3.jpg";				// jpg
	std::string test_image_2	= "../../../Work Experience/JPEG_Images/img5.jpg";				// jpg
	std::string bunny_images	= "../../../Work Experience/JPEG_Images/sample_animation/img"; 	// jpeg

	/*
	 * Handling command line arguments
	 */
	if (argc != 2) {
		std::cerr << "usage: " + std::string(argv[0]) + " <port>" << std::endl;
		exit(1);
	}
	portno = atoi(argv[1]);

	/*
	 * loading images from files into byte arrays
	 */
	std::vector<std::string> test_images(2);
	test_images[0] = LoadImage(test_image_1);
	test_images[1] = LoadImage(test_image_2);
	std::vector<std::string> animation = LoadMultipleImages(bunny_images, 53, "jpeg");
	std::vector<std::string> vr_video = LoadMultipleImages(vr_images, 1219, "jpg");

	/*
	 * socket: create the parent socket
	 */
	if ((sockfd = socket(AF_INET, SOCK_DGRAM, 0)) < 0)
		error((const char*)"ERROR opening socket");

	/* setsockopt: Handy debugging trick that lets us
	 * rerun the server immediately after we kill it;
	 * otherwise we have to wait about 20 secs.
	 * Eliminates "ERROR on binding: Address already 
	 * in use" error.
	 */
	optval = 1;
	setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, 
		(const void *)&optval , sizeof(int));

	/*
	 * build the server's Internet address
	 */
	bzero((char *) &serveraddr, sizeof(serveraddr));
	serveraddr.sin_family = AF_INET;
	serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);
	serveraddr.sin_port = htons((unsigned short)portno);

	/* 
	 * bind: associate the parent socket with a port 
	 */
	if (bind(sockfd, (struct sockaddr *) &serveraddr, 
		sizeof(serveraddr)) < 0) 
		error((const char*)"ERROR on binding");

	/*
	 * Main loop: wait for datagram, then echo it
	 */
	clientlen = sizeof(clientaddr);
	while (1) {

		/*
		 * recvfrom: receive a UDP datagram from a client
		 */
		bzero(buf, BUFSIZE);
		n = recvfrom(sockfd, buf, BUFSIZE, 0,
			(struct sockaddr *)&clientaddr, &clientlen);
		if (n < 0)
			error((const char*)"ERROR in recvfrom");

		/*
		 * gethostbyaddr: determine who sent the datagram
		 */
		hostp = gethostbyaddr((const char *)&clientaddr.sin_addr.s_addr,
			sizeof(clientaddr.sin_addr.s_addr), AF_INET);
		if (hostp == NULL) 
			error((const char*)"ERROR on gethostbyaddr");
		hostaddrp = inet_ntoa(clientaddr.sin_addr);
		if (hostaddrp == NULL)
			error((const char*)"ERROR on inet_ntoa\n");
		printf("Server has recevied datagram from %s (%s:%d)\n",
			hostp->h_name, hostaddrp, portno);
		printf("Server has receieved %d/%d bytes: %s\n", (int)strlen(buf), n, buf);

		/*
		 * Handling multiple clients at the same time
		 * NOTE: Currently unable to close processes because that would require the
		 * client to send a message to close the process but the server would then 
		 * need to figure out what process to close and idk how to differentiate them
		 * from each other currently...
		 */
		std::string recvStr(buf);
		if (recvStr == "Send me stuff!") {
			pid_t process = fork();
			if (process < 0)
				error((const char*)"ERROR in fork");
			else if (process == 0)
				ServeClient(process, test_images, animation, vr_video, hostaddrp, portno, 
					sockfd, clientaddr, clientlen);
		}

		/*
		 * sendto: echo the input back to the client
		 */
		n = sendto(sockfd, buf, strlen(buf), 0,
			(struct sockaddr*)&clientaddr, clientlen);
		if (n < 0)
			error((const char*)"ERROR in sendto");
	}

	return 0;
}