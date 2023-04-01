import * as React from 'react';
import {useHistory} from 'react-router-dom'
import {useForm} from 'react-hook-form'
import {Form} from 'reactstrap'
type FormData ={
    search:string
}
export const Search =()=>{
    const history = useHistory()
const {register,formState:{errors},watch,handleSubmit} =useForm<FormData>({
    mode:"onBlur"
})
watch()
const submitted =({search}:FormData)=>{
    
history.push(`/search?${search}`)
}
    return (
        < Form onSubmit={handleSubmit(submitted)} >
            
            <input {...register('search',{maxLength:10,required:true})} type="text" placeholder="type to search"></input>
            {errors.search && errors.search.type==="maxLength" ? 
            <label htmlFor="search"> hooooly shit ,Not allowed all this character</label>:''}
            <input type="submit"/>
        </Form>
    )
}