import React, { useEffect, useState, useRef } from 'react';
import { Typography, Grid, Card, CardContent, CardMedia, Container, Hidden, Button, Dialog, DialogContent, DialogTitle, Snackbar, CssBaseline, LinearProgress, MenuItem, Box, TextField, FormControl, InputLabel, Select } from '@material-ui/core';
import MuiAlert from '@material-ui/lab/Alert';
import useStyles from './ReservationListStyles';
import ReservationDetails from './Sections/ReservationDetails';
import { axiosApiInstance as API } from '../../../utils/axiosConfig';
import { useRecoilState, useRecoilValue } from 'recoil';
import { reservationsAtom, userAtom } from '../../../atoms/atoms';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

function Alert(props) {
    return <MuiAlert elevation={6} variant="filled" {...props} />;
}

export default function ReservationList() {
    const [reservations, setReservations] = useRecoilState(reservationsAtom);
    const [reservationId, setReservationId] = useState(0);

    const [dialogOpen, setDialogOpen] = useState(false);
    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [loading, setLoading] = useState(false);

    const [pageNumber, setPageNumber] = useState(1);
    const [numberOfPages, setNumberOfPages] = useState(0);
    const [pageSize] = useState(6);

    const [sortBy, setSortBy] = useState('');
    const [sortByOpen, setSortByOpen] = useState(false);

    const [keyword, setKeyword] = useState('');
    const [debouncedValue, setDebouncedValue] = useState('');

    const classes = useStyles();

    const user = useRecoilValue(userAtom);

    const pages = new Array(numberOfPages).fill(null).map((v, i) => i);

    useEffect(() => {
        if (user.role === 'Admin' || 'User') {
            setLoading(true);
            fetchReservations();
        }
    }, [setReservations, user.role, pageNumber, pageSize, sortBy, debouncedValue]);


    const fetchReservations = () => {
        API.get(`Bookings/paginated?pageNumber=${pageNumber}&pageSize=${pageSize}&sort=${sortBy}&keyword=${debouncedValue}`)
                .then((response) => {
                    if (response.status === 200) {
                        setReservations(response.data);
                        setNumberOfPages(response.data[0]?.totalPages);
                        setLoading(false);
                    }
                })
                .catch((error) => {
                    setLoading(false);
                    alert(error);
                });
        
    }

    const goToPrevious = () => {
        setPageNumber(Math.max(1, pageNumber - 1));
    };

    const goToNext = () => {
        setPageNumber(Math.min(numberOfPages, pageNumber + 1));
    };

    const handleKeywordChange = (event) => {
        setKeyword(event.target.value);
        setTimeout(() => {
            setDebouncedValue(event.target.value);
        }, 1500);
    };

    const handleSortByClose = () => {
        setSortByOpen(false);
    };

    const handleSortByOpen = () => {
        setSortByOpen(true);
    };

    const handleSortByChange = (event) => {
        setSortBy(event.target.value);
    };

    // const handleDialogOpen = (event) => {
    //     setDialogOpen(true);
    //     // setReservationId(event.currentTarget.id * 1);
    //     setReservationId(event.currentTarget.id);
    // };

    const updateBookingStatus =(reservationId,status)=>{
        API.put(`Bookings/${reservationId}/status`, status, {
            headers: { "Content-Type": "application/json" },
        })
        .then((response) => {
            if (response.status === 200) {
                fetchReservations();
                setSnackbarOpen(true);
            }
            setLoading(false);
        })
        .catch((error) => {
            setLoading(false);
            alert(error.message || "An error occurred");
        });
    }


    const handleCancelReservation = (reservationId) => {
        setLoading(true);
        updateBookingStatus(reservationId,"INREVIEW");

        // API.delete(`Bookings/${reservationId}`)
        //     .then((response) => {
        //         if (response.status === 204) {
        //             setReservations((prevReservations) => prevReservations.filter((res) => res.id !== reservationId));
        //             setSnackbarOpen(true);
        //         }
        //         setLoading(false);
        //     })
        //     .catch((error) => {
        //         setLoading(false);
        //         alert(error);
        //     });
    };

    const handleDialogClose = () => {
        setDialogOpen(false);
    };

    const handleSnackbar = () => {
        setSnackbarOpen(true);
    };

    const handleSnackbarClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        setSnackbarOpen(false);
    };
    // const printRef = useRef({});

    const handlePrint = async (reservationId) => {
        const reservationToPrint = reservations.find((res) => res.id === reservationId);

        if (reservationToPrint) {
            const html = `
              <div style="padding: 20px; font-size: 14px; text-align: center;">
                  <p>Booking ID# ${reservationToPrint.id}</p>
                  <h4>Customer: ${reservationToPrint.customerName}</h4>
                  <h4>Movie: ${reservationToPrint.movieName}</h4>
                  <p>Date: ${new Date(reservationToPrint.bookingDate).toLocaleDateString('en-US')}</p>
                  <p>Seat Numbers: ${reservationToPrint.seatNumbers}</p>
                  <p>Total Price: $${reservationToPrint.totalPrice}</p>
                  <p>Location: $${reservationToPrint.location}</p>
              </div>
          `;
            printHTMLStringToPDF(html);
        }
    };

    const printHTMLStringToPDF = (htmlString) => {
        //const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        // Create a hidden container to render the HTML
        const hiddenContainer = document.createElement('div');
        hiddenContainer.style.position = 'absolute';
        hiddenContainer.style.left = '-9999px';
        hiddenContainer.innerHTML = htmlString;
        document.body.appendChild(hiddenContainer);

        // Use html2canvas to render the hidden HTML to a canvas
        html2canvas(hiddenContainer).then((canvas) => {
            const imgData = canvas.toDataURL('image/png');

            // Calculate image size to fit PDF
            const imgWidth = 190; // Fit width to A4 size
            const imgHeight = (canvas.height * imgWidth) / canvas.width;

            // Add image to PDF and save
            doc.addImage(imgData, 'PNG', 10, 10, imgWidth, imgHeight);
            doc.save('download.pdf');

            // Clean up hidden container
            document.body.removeChild(hiddenContainer);
        });
    };
    return (
        <div>
            {user.role === 'Admin' || 'User' ? (
                <React.Fragment>
                    <CssBaseline />
                    <main>
                        <div className={classes.heroContent}>
                            <Container maxWidth="sm">
                                <Typography component="h1" variant="h2" align="center" color="textPrimary" gutterBottom>
                                    Bookings
                                </Typography>
                            </Container>
                        </div>
                    </main>

                    <CssBaseline />
                    <div className={classes.container}>
                        <Container maxWidth="md">
                            {/* <div className={classes.sortBy_search}>
                                <TextField variant="outlined" label="Search" value={keyword} placeholder="Customer Name" onChange={handleKeywordChange} />

                                <FormControl className={classes.formControl}>
                                    <InputLabel shrink id="open-select-label">
                                        Sort By
                                    </InputLabel>
                                    <Select labelId="open-select-label" id="open-select" open={sortByOpen} onClose={handleSortByClose} onOpen={handleSortByOpen} value={sortBy} onChange={handleSortByChange}>
                                        <MenuItem value="">
                                            <em>Default (latest first)</em>
                                        </MenuItem>
                                        <MenuItem value="created_oldest">Created (oldest first)</MenuItem>
                                        <MenuItem value="movie_name">Movie (A to Z)</MenuItem>
                                        <MenuItem value="customer_name">Customer Name (A to Z)</MenuItem>
                                    </Select>
                                </FormControl>
                            </div> */}

                            {loading ? (
                                <LinearProgress />
                            ) : (
                                <Grid container spacing={4}>
                                    <Snackbar anchorOrigin={{ vertical: 'top', horizontal: 'center' }} open={snackbarOpen} autoHideDuration={6000} onClose={handleSnackbarClose}>
                                        <Alert onClose={handleSnackbarClose} severity="success">
                                            Successfully canceled the ticket!
                                        </Alert>
                                    </Snackbar>

                                    {reservations?.map((item) => (
                                        <Grid item key={item.id} xs={12} md={6}>
                                            <Card className={classes.card}>
                                                <div className={classes.cardDetails}>
                                                    <CardContent>
                                                        <Typography component="h2" variant="caption">
                                                            Booking ID# {item.id}
                                                        </Typography>
                                                        
                                                        <Typography variant="h5">{item.movieName}</Typography>
                                                        <Typography variant="subtitle1" gutterBottom>
                                                            {new Date(item.bookingDate).toLocaleDateString('en-US')}  |  
                                                            {item.reservationTime}
                                                            {/* {new Intl.DateTimeFormat('en-US', {
                                                                year: 'numeric',
                                                                month: 'long',
                                                                day: 'numeric',
                                                                // hour: 'numeric',
                                                                // minute: 'numeric',
                                                            }).format(new Date(item.bookingDate))} */}
                                                        </Typography>
                                                        <Typography variant="subtitle1"><b>Name</b> :{item.customerName}</Typography>
                                                        <Typography variant="subtitle1"><b>Seat Numbers</b> : {item.seatNumbers}</Typography>
                                                        <Typography variant="subtitle1"><b>Total Price</b> : {item.totalPrice}</Typography>
                                                        <Typography variant="subtitle1"><b>Location</b> : {item.location}</Typography>
                                                        {/* IF THE USER IS asdmin and refunded the big text asrefunded*/}
                                                        { (item.status === 'REFUNDED' || item.status === 'INREVIEW' ) && (
                                                            <Typography variant="subtitle1" color="secondary">
                                                                {item.status}
                                                            </Typography>
                                                        )}

                                                        {/* {user.role === 'User' && item.status === 'REFUNDED' && (
                                                            <Typography variant="subtitle1" color="secondary">
                                                                REFUNDED
                                                            </Typography>
                                                        )} */}


                                                        {/* <Button variant="contained" color="primary" onClick={handleDialogOpen} id={item.id}>
                                                            Details
                                                        </Button> */}
                                                        {/* if the booking date is today, then disable cancel button */}
                                                        {/* {(new Date(item.bookingDate) > new Date()) && ( */}
                                                        {user.role === 'User' && item.status === 'ACTIVE' && (
                                                            <Button variant="contained" color="secondary" onClick={() => handleCancelReservation(item.id)}>
                                                                Cancel
                                                            </Button>
                                                         )}   

                                                        {/* if user is adming and status is REVIEW then show Approve cancel button */}
                                                   
                                                        {user.role === 'Admin' && item.status === 'INREVIEW' && (
                                                            <Button variant="contained" color="primary" onClick={() => updateBookingStatus(item.id,"REFUNDED")}>
                                                                Approve Cancel
                                                            </Button>
                                                        )}
                                                        
                                                        
                                                        
                                                        &nbsp;&nbsp;
                                                        <Button variant="contained" color="primary" onClick={() => handlePrint(item.id)}>
                                                            Print
                                                        </Button>
                                                    </CardContent>
                                                </div>
                                                <Hidden xsDown>{item.movieImageUrl && <CardMedia className={classes.cardMedia} image={`https://localhost:5001/${item.movieImageUrl.slice(1)}`} title="unsplash image" />}</Hidden>
                                            </Card>
                                        </Grid>
                                    ))}
                                </Grid>
                            )}
                            <Box className={classes.pagination}>
                                <Button variant="outlined" color="primary" onClick={goToPrevious} className={classes.previous}>
                                    Previous
                                </Button>
                                {pages.map((pageIndex, index) => (
                                    <Button variant="outlined" key={index} onClick={() => setPageNumber(pageIndex + 1)}>
                                        {pageIndex + 1}
                                    </Button>
                                ))}
                                <Button variant="outlined" color="primary" onClick={goToNext} className={classes.next}>
                                    Next
                                </Button>
                            </Box>

                            {/* <Dialog open={dialogOpen} onClose={handleDialogClose}>
                                <DialogTitle id="simple-dialog-title">Booking Details</DialogTitle>
                                <DialogContent>
                                    <ReservationDetails reservationId={reservationId} handleDialogClose={handleDialogClose} handleSnackbar={handleSnackbar} />
                                </DialogContent>
                            </Dialog> */}
                        </Container>
                    </div>
                </React.Fragment>
            ) : (
                <div>
                    <CssBaseline />
                    <main>
                        <div className={classes.heroContent}>
                            <Container maxWidth="sm">
                                <Typography component="h1" variant="h2" align="center" color="textPrimary" gutterBottom>
                                    Admin Only Page
                                </Typography>
                            </Container>
                        </div>
                    </main>
                </div>
            )}
        </div>
    );
}
