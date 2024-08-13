﻿using Application.Interfaces;
using Common.Exceptions;
using Domain.Models.Entities;
using Domain.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retorna todas as categorias.
        /// </summary>
        /// <returns>Uma lista de categorias.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retorna uma categoria com o ID especificado.
        /// </summary>
        /// <param name="id">ID da categoria a ser retornada.</param>
        /// <returns>Categoria correspondente ao ID fornecido.</returns>
        /// <response code="200">Categoria foi encontrada e retornada com sucesso.</response>
        /// <response code="404">Categoria com o ID fornecido não foi encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                return Ok(category);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adiciona uma nova categoria.
        /// </summary>
        /// <param name="categoryRequestModel">Nova categoria a ser adicionada.</param>
        /// <returns>Nova categoria adicionada.</returns>
        /// <response code="201">Categoria adicionada com sucesso.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<Category>> PostCategory(CategoryRequestModel categoryRequestModel)
        {
            try
            {
                var newCategory = await _categoryService.CreateCategory(categoryRequestModel);
                return CreatedAtAction("GetCategoryById", new { id = newCategory.Id }, newCategory);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.ValidationErrors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser atualizada.</param>
        /// <param name="categoryRequestModel">Dados atualizados da categoria.</param>
        /// <returns>Retorna NoContent se a atualização for bem-sucedida.</returns>
        /// <response code="204">Categoria atualizada com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutCategory(int id, CategoryRequestModel categoryRequestModel)
        {
            try
            {
                await _categoryService.UpdateCategory(id, categoryRequestModel);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.ValidationErrors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Deleta uma categoria com o ID especificado.
        /// </summary>
        /// <param name="id">ID da categoria a ser deletada.</param>
        /// <returns>Nenhum conteúdo.</returns>
        /// <response code="204">Categoria foi deletada com sucesso.</response>
        /// <response code="404">Categoria com o ID fornecido não foi encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
