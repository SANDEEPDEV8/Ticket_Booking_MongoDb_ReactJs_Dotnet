import React, { useEffect, useState } from 'react';
import { Paper, Typography, Grid, CssBaseline, Container, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, IconButton, Dialog, DialogContent, Snackbar } from '@material-ui/core';
import { Alert } from '@material-ui/lab';
import useStyles from './MovieDetailsStyles';
import { Delete as DeleteIcon } from '@material-ui/icons';
import axios from 'axios';
import { useRecoilState, useRecoilValue } from 'recoil';
import { movieDetailAtom, userAtom } from '../../atoms/atoms';
import MovieSubDetails from './Sections/MovieSubDetails';
import AddScheduleForm from './Sections/AddScheduleForm';
import MovieBookingForm from './Sections/MovieBookingForm';

export default function MovieDetails(props) {
    const classes = useStyles();
    const baseURL = 'https://localhost:5001/api/';

    const [movieDetail, setMovieDetail] = useRecoilState(movieDetailAtom);
    const [scheduleDetail, setScheduleDetail] = useState({});
    const [snackbarOpen, setSnackbarOpen] = React.useState(false);

    const user = useRecoilValue(userAtom);
    const movieId = props.match.params.id;
    const [openScheduleForm, setOpenScheduleForm] = useState(false);
    const [schedules, setSchedules] = useState([]);

    const [openBookDialog, setOpenBookDialog] = useState(false);

    useEffect(() => {
        if (openScheduleForm) {
            return;
        }
        axios.get(`${baseURL}movies/${movieId}`).then((response) => {
            if (response.status === 200) {
                setMovieDetail(response.data);
            }
        });
        axios.get(`${baseURL}schedules/details/${movieId}`).then((response) => {
            if (response.status === 200) {
                setSchedules(response.data);
            }
        });
    }, [movieId, setMovieDetail, openScheduleForm]);

    const handleDeleteSchedule = (scheduleId) => {
        axios.delete(`${baseURL}schedules/${scheduleId}`).then((response) => {
            if (response.status === 204) {
                setSchedules((prevSchedules) => prevSchedules.filter((schedule) => schedule.scheduleId !== scheduleId));
            }
        });
    };
    const onBook = (schedule) => {
        console.log(schedule);
        setScheduleDetail(schedule);
        // setMovieDetail({ ...movieDetail, schedule });
        setOpenBookDialog(true);
    };

    const handleSnackbarClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        setSnackbarOpen(false);
    };

    return (
        <React.Fragment>
            <CssBaseline />
            <Snackbar anchorOrigin={{ vertical: 'top', horizontal: 'center' }} open={snackbarOpen} autoHideDuration={6000} onClose={handleSnackbarClose}>
                <Alert onClose={handleSnackbarClose} severity="success">
                    Successfully book your ticket!
                </Alert>
            </Snackbar>
            <div className={classes.container}>
                <Container maxWidth="lg">
                    <Paper
                        className={classes.mainFeaturedPost}
                        style={
                            movieDetail.imageUrl && {
                                backgroundImage: `url(
          https://localhost:5001/${movieDetail.imageUrl.slice(1)}
          )`,
                            }
                        }>
                        {/* Increase the priority of the hero background image */}
                        {movieDetail.imageUrl && <img style={{ display: 'none' }} src={`https://localhost:5001/${movieDetail.imageUrl.slice(1)}`} alt={movieDetail.name} />}
                        <div className={classes.overlay} />
                        <Grid container>
                            <Grid item md={6}>
                                <div className={classes.mainFeaturedPostContent}>
                                    <Typography component="h1" variant="h3" color="inherit" gutterBottom>
                                        {movieDetail.name}
                                    </Typography>
                                    <Typography variant="h5" color="inherit" paragraph>
                                        {movieDetail.description}
                                    </Typography>
                                </div>
                            </Grid>
                        </Grid>
                    </Paper>
                    <MovieSubDetails movieDetail={movieDetail} movieId={movieId} />
                    {user.role === 'Admin' && (
                        <div>
                            <Grid container justifyContent="flex-end">
                                <Button
                                    variant="contained"
                                    color="primary"
                                    onClick={() => {
                                        setOpenScheduleForm(true);
                                    }}
                                    className={classes.addScheduleButton}>
                                    Add Schedule
                                </Button>
                            </Grid>
                            </div>
                        )}
                        <div>
                            <div style={{margin:"20px 0"}}></div>

                            {/* show heading called title */}
                            <Typography variant="h6" gutterBottom>
                                Schedules
                            </Typography>

                            <TableContainer component={Paper} style={{ marginTop: '20px' }}>
                                <Table>
                                    <TableHead>
                                        <TableRow>
                                            <TableCell>Date</TableCell>
                                            <TableCell>Price</TableCell>
                                            <TableCell>Start Time</TableCell>
                                            <TableCell>End Time</TableCell>
                                            <TableCell>Screen Number</TableCell>
                                            <TableCell>Location</TableCell>
                                            <TableCell>Actions</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {schedules.map((schedule) => (
                                            <TableRow key={schedule.scheduleId}>
                                                <TableCell>{schedule.date}</TableCell>
                                                <TableCell>${schedule.price}</TableCell>
                                                <TableCell>{schedule.startTime}</TableCell>
                                                <TableCell>{schedule.endTime}</TableCell>
                                                <TableCell>{schedule.screenNumber}</TableCell>
                                                <TableCell>{schedule.location}</TableCell>
                                                <TableCell>
                                                {user.role === 'Admin' && (
                                                    <IconButton onClick={() => handleDeleteSchedule(schedule.scheduleId)}>
                                                        <DeleteIcon color="secondary" />
                                                    </IconButton>
                                                )}
                                                    <Button variant="contained" color="primary" style={{ marginLeft: '10px' }} onClick={() => onBook(schedule)}>
                                                        Book
                                                    </Button>
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </div>
                   

                    <AddScheduleForm
                        open={openScheduleForm}
                        handleClose={() => {
                            setOpenScheduleForm(false);
                        }}
                        movieId={movieId}
                    />
                </Container>

                <Dialog open={openBookDialog} onClose={() => setOpenBookDialog(false)}>
                    <DialogContent>
                        <MovieBookingForm movieDetail={movieDetail} scheduleDetail={scheduleDetail} movieId={movieId} handleClose={() => setOpenBookDialog(false)} handleSnackbar={handleSnackbarClose} />
                    </DialogContent>
                </Dialog>
            </div>
        </React.Fragment>
    );
}
